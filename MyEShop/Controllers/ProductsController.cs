using Microsoft.AspNet.Identity;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using MyEShop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {

            return View(await db.Products.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.ShowBidBox = "false";

            var userId = User.Identity.GetUserId();
            if (product.UserId == userId)
            {
                ViewBag.ShowBidBox = "true";

            }

            var pId = product.Id;
            var LstImage = db.ProductImages.Where(p => p.ProductId == pId).ToList();
            if (LstImage != null)
            {
                ViewBag.ProductImages = LstImage;
            }

            product.Visit++;
            db.SaveChanges();

            return View(product);
        }

        public ActionResult Filter(List<int> filterItemschk, decimal maxPrice = 100000, int CategoryId = 0, decimal minPrice = 5, int page = 1)
        {
            List<Product> productsVM = new List<Product>();

            var _minPrice = minPrice >= 5 ? minPrice : 5;
            var _maxPrice = maxPrice >= 10 ? maxPrice : 100000;


            if (CategoryId == 0)
            {
                return RedirectToAction("Index");
            }

            var products = db.Products.Include("FilterItems").Where(p => (p.CategoryId == CategoryId) && (p.EndDate == null || p.EndDate > DateTime.Now));

            if (filterItemschk != null)
            {
                if (filterItemschk.Count() > 0)
                {
                    var filterItems = db.FilterItems.Where(f => filterItemschk.Contains(f.Id));
                    foreach (var item in products)
                    {
                        if (item.FilterItems.Intersect(filterItems).Count() == filterItems.Count())
                        {
                            productsVM.Add(item);
                        }
                    }
                }
            }
            else
            {
                productsVM.AddRange(products);
            }

            if (_minPrice >= 5 && _maxPrice >= 10)
            {
                var q = productsVM.Where(p => p.Price >= _minPrice && p.Price <= _maxPrice).ToList();

                productsVM.Clear();
                productsVM.AddRange(q);
            }

            int take = 1;
            var count = productsVM.Count();

            int skip = 0;
            if (count > take)
            {
                skip = (take * page) - take;
            }


            var TotalPages = Math.Ceiling((decimal)(count / take));
            ViewBag.TotalPages = TotalPages;

            var _page = page <= TotalPages ? page : TotalPages;

            ViewBag.Page = _page;
            ViewBag.CategoryId = CategoryId;

            return PartialView("_ProductItems", productsVM.OrderBy(p => p.Id).Skip(skip).Take(take));
        }

        public ActionResult CategoryItems(int id)
        {
            ViewBag.CategoryId = id;
            return View();

        }



        public ActionResult Search(string searchText, int searchPage = 1, int PageStatus = 1)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                RedirectToAction("Index", "Home");
            }

            List<Product> productsVM = new List<Product>();

            productsVM = db.Products
                .Where(p => (p.Name.Contains(searchText) || p.Text.Contains(searchText)) && (p.EndDate == null || p.EndDate > DateTime.Now)).ToList();

            int take = 1;
            var count = productsVM.Count();

            int skip = 0;
            if (count > take)
            {
                skip = (take * searchPage) - take;
            }

            var TotalPages = Math.Ceiling((decimal)(count / take));
            ViewBag.TotalPages = TotalPages;
            var _page = searchPage <= TotalPages ? searchPage : TotalPages;
            ViewBag.SearchPage = _page;
            ViewBag.SearchTerm = searchText;
            ViewBag.PageStatus = 2;

            if (PageStatus == 2)
            {
                return PartialView("_SearchItems", productsVM.OrderBy(p => p.Id).Skip(skip).Take(take));
            }

            return View(productsVM.OrderBy(p => p.Id).Skip(skip).Take(take));
        }

        public ActionResult DisplayShoppingCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                var items = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();

                CartVM cart = new CartVM();

                cart.Count = items.Count();

                foreach (var item in items)
                {
                    cart.TotalPrice += item.Price * item.Count;
                }

                foreach (var cartitem in items)
                {
                    CartItemVM cartItem = new CartItemVM()
                    {
                        Count = cartitem.Count,
                        Price = cartitem.Price,
                        ProductName = cartitem.ProductName,
                        WebId = cartitem.Id
                    };
                    //cart.CartItems.Add(cartitem);
                }
                return View();
            }
            // return to index
            return RedirectToAction("Index", "Home");
        }


        public JsonResult ItemsInCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId).Count();
                return Json(new { ItemsInCart = itemsInCart }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AddToCart(int pId = 0, int count = 0)
        {

            if (User.Identity.IsAuthenticated)
            {

                if (pId == 0)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                var userId = User.Identity.GetUserId();
                var product = db.Products.Where(p => p.Id == pId).SingleOrDefault();
                var numberAvailable = product.Count;

                if (numberAvailable >= count)
                {

                    ShoppingCart shoppingCart = new ShoppingCart()
                    {
                        Count = count,
                        Date = DateTime.Now,
                        ProductId = pId,
                        UserId = userId,
                        Status = false,
                        Price = product.Price,
                        ProductName = product.Name
                    };

                    product.Count -= count;
                    db.Entry(product).State = EntityState.Modified;
                    db.ShoppingCart.Add(shoppingCart);
                    db.SaveChanges();

                    return Json(new
                    {
                        redirectUrl = Url.Action("ItemsInCart", "Products")
                    });
                }
                else
                {
                    return Json(new { Message = "There is not enough items available." }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MyProducts(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var myProducts = db.Products.Where(p => p.UserId == userId && p.EndDate == null).ToList();

                int take = 1;
                var count = myProducts.Count();
                int skip = 0;
                if (count > take)
                {
                    skip = (take * page) - take;
                }

                var TotalPages = Math.Ceiling((decimal)(count / take));
                ViewBag.TotalPages = TotalPages;
                var _page = page <= TotalPages ? page : TotalPages;
                ViewBag.Page = _page;
                ViewBag.MethodName = "MyProducts";
                return View("MyProductsForSale", myProducts.OrderBy(p => p.Id).Skip(skip).Take(take));
            }


            return RedirectToAction("Index", "Manage");
        }

        public ActionResult ProductsForAuction(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var myProducts = db.Products.Where(p => p.UserId == userId && p.EndDate != null).ToList();


                int take = 1;
                var count = myProducts.Count();
                int skip = 0;
                if (count > take)
                {
                    skip = (take * page) - take;
                }

                var TotalPages = Math.Ceiling((decimal)(count / take));
                ViewBag.TotalPages = TotalPages;
                var _page = page <= TotalPages ? page : TotalPages;
                ViewBag.Page = _page;
                ViewBag.MethodName = "ProductsForAuction";
                return View("MyProductsForSale", myProducts.OrderBy(p => p.Id).Skip(skip).Take(take));
            }

            return RedirectToAction("Index", "Manage");

        }



        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {

                if (User.IsInRole("Seller"))
                {
                    ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                    {
                        Text = c.CategoryName,
                        Value = c.Id.ToString()
                    }).ToList();
                    return View();
                }
                else
                {
                    return Content("You are not a seller.");
                }

            }
            return RedirectToAction("Index", "Manage");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, HttpPostedFileBase Image, HttpPostedFileBase[] carouselImage, List<int?> filterItemschk)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (User.IsInRole("Seller"))
                {
                    var userId = User.Identity.GetUserId();
                    var seller = db.Users.Where(u => u.Id == userId).SingleOrDefault();
                    if (seller.BankAcount == null || seller.BankName == null)
                    {
                        ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                        {
                            Text = c.CategoryName,
                            Value = c.Id.ToString()
                        }).ToList();

                        ViewBag.Error = "You have to fill out your Bank Name and account to be able to sell products";
                        return View(product);
                    }
                    var randomNameGenerate = new Random();

                    if (ModelState.IsValid)
                    {
                        var filterItems = db.FilterItems.Where(f => filterItemschk.Any(fi => fi == f.Id));

                        product.UserId = userId;
                        product.Visit = 0;
                        product.date = DateTime.Now;
                        if (product.FilterItems == null)
                        {
                            product.FilterItems = new Collection<FilterItem>();
                        }
                        foreach (var fi in filterItems)
                        {
                            product.FilterItems.Add(fi);
                        }

                        //check image is valid
                        product.Image = randomNameGenerate.Next().ToString() + ".jpg";
                        Image.SaveAs(Server.MapPath("~") + "Content/Product-Pictures/" + product.Image);
                        //product.Image = Image.FileName;

                        db.Products.Add(product);
                        if (Convert.ToBoolean(await db.SaveChangesAsync()))
                        {
                            ViewBag.Message = "Item Created Successfully.";
                        }
                        else
                        {
                            return View(product);
                        }


                        var productId = product.Id;

                        //if (carouselImage.Count() > 0)
                        //{
                        //    foreach (var c in carouselImage)
                        //    {
                        //        //Image Validation goes here

                        //    }

                        //}

                        if (carouselImage.Count() > 0)
                        {
                            List<ProductImage> lstImage = new List<ProductImage>();
                            foreach (var c in carouselImage)
                            {
                                if (c != null)
                                {
                                    ProductImage img = new ProductImage();
                                    img.ImageName = randomNameGenerate.Next().ToString() + ".jpg";
                                    c.SaveAs(Path.Combine(Server.MapPath("~") + "Content/Product-Carousel/" + img.ImageName));
                                    img.ProductId = productId;

                                    lstImage.Add(img);
                                }
                            }

                            db.ProductImages.AddRange(lstImage);
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index", "Manage");
                    }
                    ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                    {
                        Text = c.CategoryName,
                        Value = c.Id.ToString()
                    }).ToList();
                    return View(product);
                }
            }
            return RedirectToAction("Index", "Manage");

        }


        public ActionResult ProductsISold()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var items = db.Sales.Include("Product").Where(s => s.Product.UserId == userId).ToList();
                return View(items);
            }
            return RedirectToAction("Index", "Manage");
        }


        public ActionResult SaleDetails(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id > 0)
                {
                    var userId = User.Identity.GetUserId();
                    var item = db.Sales.Include("Product").Include("User").Where(s => s.Product.UserId == userId && s.Id == id).SingleOrDefault();

                    ViewBag.OrderStatus = new SelectList(Enum.GetValues(typeof(SalesStatus)), item.StatusId);
                    return View(item);
                }

            }
            return RedirectToAction("Index", "Manage");

        }


        public ActionResult ChangeStatus(int Id, SalesStatus StatusId, int TrackingCode)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var sale = db.Sales.Where(s => s.Id == Id && s.Product.UserId == userId).SingleOrDefault();

                sale.StatusId = StatusId;
                sale.TrackingCode = TrackingCode;

                //db.Sales.Attach(sale);
                db.SaveChanges();
                return RedirectToAction("ProductsISold", "Products");
            }
            return RedirectToAction("Index", "Manage");

        }


        public ActionResult MyBill()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var soldItems = db.Sales.Where(s => s.Product.UserId == userId && s.Payed == true).ToList();

                var myCurrent = db.Bills.Where(b => b.UserId == userId).SingleOrDefault();

                if (myCurrent != null)
                {
                    var totalPrice = soldItems.Sum(s => s.Price);
                    BillVM billVM = new BillVM()
                    {
                        LastRec = myCurrent.LastRec,
                        TotalRec = myCurrent.TotalRec,
                        Available = Convert.ToInt32(totalPrice) - myCurrent.TotalRec,
                        Withdrawable = (Convert.ToInt32(totalPrice) - myCurrent.TotalRec) - 100,
                        TimeOfLastRec = myCurrent.TimeOfLastRec ?? DateTime.Now
                    };

                    return View(billVM);
                }
                else
                {
                    BillVM billVM = new BillVM()
                    {
                        LastRec = 0,
                        TotalRec = 0,
                        Available = 0,
                        Withdrawable = 0
                    };

                    return View(billVM);
                }

            }
            return RedirectToAction("Index", "Manage");
        }


        public ActionResult PayMe(int PayMeAmount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var myCurrent = db.Bills.Where(b => b.UserId == userId).SingleOrDefault();

                if (myCurrent != null)
                {
                    var soldItems = db.Sales.Where(s => s.Product.UserId == userId && s.Payed == true).ToList();
                    var totalPrice = soldItems.Sum(s => s.Price);

                    if (totalPrice > PayMeAmount)
                    {
                        myCurrent.PayMeAmount = PayMeAmount;
                        myCurrent.PayMe = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("You Do not have enough money in your account.");
                    }
                    return RedirectToAction("MyBill", "Products");

                }
                else
                {
                    return Content("You Do not have enough money in your account.");
                }
            }
            return RedirectToAction("Index", "Manage");
        }





        public async Task<ActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Include("FilterItems").SingleOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                }).ToList();
                return View(product);
            }

            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, HttpPostedFileBase NewImage, List<int?> filterItemschk)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (ModelState.IsValid)
                {
                    var userId = User.Identity.GetUserId();
                    var seller = db.Users.Where(u => u.Id == userId).SingleOrDefault();

                    product.UserId = userId;
                    product.User = seller;

                    var tmpItems = db.Products.Include("FilterItems").Where(p => p.Id == product.Id).SingleOrDefault();
                    var tmpFilters = tmpItems.FilterItems.ToList();

                    tmpItems.Name = product.Name;
                    tmpItems.Onsale = product.Onsale;
                    tmpItems.OnSalePrice = product.OnSalePrice;
                    tmpItems.CategoryId = product.CategoryId;
                    tmpItems.Count = product.Count;
                    tmpItems.Description = product.Description;
                    tmpItems.EndDate = product.EndDate;
                    tmpItems.Price = product.Price;
                    tmpItems.Text = product.Text;
                    tmpItems.weight = product.weight;


                    //db.FilterItems.RemoveRange(tmpItems);
                    foreach (var item in tmpFilters)
                    {
                        tmpItems.FilterItems.Remove(item);

                    }

                    if (filterItemschk != null)
                    {
                        var filterItems = db.FilterItems.Where(f => filterItemschk.Any(fi => fi == f.Id));
                        tmpItems.FilterItems = new Collection<FilterItem>();
                        foreach (var fi in filterItems)
                        {
                            //if already exists do not change, else add/remove child items
                            //db.FilterItems.Add(fi);
                            tmpItems.FilterItems.Add(fi);
                        }
                    }

                    var randomNameGenerate = new Random();
                    if (NewImage != null)
                    {
                        tmpItems.Image = randomNameGenerate.Next().ToString() + ".jpg";
                        NewImage.SaveAs(Server.MapPath("~") + "Content/Product-Pictures/" + product.Image);
                    }

                    //db.Products.Attach(product);
                    //db.Entry(product).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        return Content(e.Message);
                    }
                    //if (Convert.ToBoolean(await db.SaveChangesAsync()))
                    //{
                    //    ViewBag.Message = "Item Created Successfully.";
                    //}
                    //else
                    //{
                    //    return View(product);
                    //}
                    return RedirectToAction("MyProducts", "Products");
                }
                ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                }).ToList();
                return View(product);
            }

            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult ManageBills(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var users = db.Bills.Include("User").Where(b => b.PayMe == true).ToList();

                    int take = 1;
                    var count = users.Count();
                    int skip = 0;
                    if (count > take)
                    {
                        skip = (take * page) - take;
                    }
                    var TotalPages = Math.Ceiling((decimal)(count / take));
                    ViewBag.TotalPages = TotalPages;
                    var _page = page <= TotalPages ? page : TotalPages;
                    ViewBag.Page = _page;

                    return View(users.OrderBy(p => p.Id).Skip(skip).Take(take));
                }
            }
            return RedirectToAction("Index", "Manage");
        }


        public ActionResult PaySeller(int amount, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var bill = db.Bills.Where(b => b.Id == id && b.PayMe == true).SingleOrDefault();
                    if (bill != null)
                    {
                        //pay the amout, change the balance
                        //Send a message to the User

                        if ((bill.TotalRec - 100) > amount)
                        {
                            bill.TotalRec -= amount;
                            bill.PayMe = false;
                            bill.PayMeAmount = 0;
                            bill.TimeOfLastRec = DateTime.Now;

                            var msg = new Message()
                            {
                                Date = DateTime.Now,
                                IsRead = false,
                                Text = "You have been paid " + amount,
                                Title = "New Payment",
                                UserRecId = bill.UserId,
                                UserRec = bill.User
                            };

                            db.Messages.Add(msg);
                            db.SaveChanges();
                        }

                        return RedirectToAction("ManageBills");
                    }
                    return RedirectToAction("ManageBills");
                }
            }
            return RedirectToAction("Index", "Manage");
        }







        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
