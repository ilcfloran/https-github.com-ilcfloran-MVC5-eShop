using Microsoft.AspNet.Identity;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using MyEShop.Web.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {

            return View(db.ShoppingCart.ToList());
        }



        public ActionResult GetShoppingCartItems()
        {
            if (User.Identity.IsAuthenticated)
            {
                CartVM cart = new CartVM();
                //List<CartItemVM> cartItemList = new List<CartItemVM>();

                var userId = User.Identity.GetUserId();
                var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();

                foreach (var item in itemsInCart)
                {
                    CartItemVM itm = new CartItemVM();
                    itm.Count = item.Count;
                    itm.Price = item.Price;
                    itm.ProductName = item.ProductName;
                    itm.WebId = item.Id;

                    cart.TotalPrice += item.Count * item.Price;
                    if (cart.CartItems == null)
                    {
                        cart.CartItems = new List<CartItemVM>();
                    }
                    cart.CartItems.Add(itm);
                }
                return View(cart);
            }


            return RedirectToAction("Index", "Home");
        }

        public JsonResult UpdateItems(int itemId, int operation)
        {


            if (itemId > 0)
            {
                var userId = User.Identity.GetUserId();
                var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();
                var item = itemsInCart.Where(i => i.Id == itemId).SingleOrDefault();


                decimal total = 0;
                foreach (var x in itemsInCart)
                {
                    total += x.Count * x.Price;
                }


                if (operation == 1)
                {
                    item.Count += 1;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    total += item.Price;

                }
                else if (operation == 0)
                {
                    if (item.Count > 0)
                    {
                        item.Count -= 1;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        total -= item.Price;
                    }
                }
                else
                {
                    return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet);
                }

                //calculate item price and total price
                var itemFee = (item.Count * item.Price);


                return Json(new { itemPrice = itemFee, totalPrice = total }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet); ;
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,ProductName,UserId,Date,Status,Count,Price")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shoppingCart);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,ProductName,UserId,Date,Status,Count,Price")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingCart);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            db.ShoppingCart.Remove(shoppingCart);
            db.SaveChanges();
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
