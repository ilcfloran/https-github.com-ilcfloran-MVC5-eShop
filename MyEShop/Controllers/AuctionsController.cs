using Microsoft.AspNet.Identity;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var auctions = db.Auctions.Include(a => a.User);
            return View(await auctions.ToListAsync());
        }

        public ActionResult AuctionByProduct(int id)
        {

            var auctions = db.Auctions.Include("User").Where(a => a.ProductId == id).ToList();
            //if (auctions.Count() == 0)
            //{
            //    return Content("No bids yet.");
            //}

            var user = db.Products.Include("User").Where(p => p.Id == id)
                .Select(p => new
                {
                    Id = p.UserId,
                    FullName = p.User.FirstName + " " + p.User.LastName,
                    MemberSince = p.User.MemberSince,
                    City = p.User.City
                })
                .SingleOrDefault();


            //var userBid = db.Auctions.Where(a => a.UserId == user.Id).SingleOrDefault();

            var biderId = User.Identity.GetUserId();
            var biderOffer = db.Auctions.Where(a => a.UserId == biderId && a.ProductId == id).Select(a => a.Price).SingleOrDefault();

            ViewBag.UserBid = biderOffer;
            ViewBag.UserName = user.FullName;
            ViewBag.MemberSince = user.MemberSince;
            ViewBag.City = user.City;
            ViewBag.AuctionCount = auctions.Count();
            ViewBag.ProductId = id;
            ViewBag.BidError = "";
            ViewBag.ErrorDisplay = "none";
            if (TempData["bidError"] != null)
            {
                ViewBag.BidError = TempData["bidError"];
                ViewBag.ErrorDisplay = "block";
            }


            return PartialView("_AuctionByProduct", auctions.OrderByDescending(a => a.Price).Take(3));
        }

        [HttpPost]
        public ActionResult Offer(int bid, int pId)
        {
            // check if user previously bid for the same product, if did, update the data
            //check if the person who added the product for auction cannot bid


            var biderId = User.Identity.GetUserId();
            var userBid = db.Auctions.Where(a => a.UserId == biderId && a.ProductId == pId).SingleOrDefault();
            var product = db.Products.Where(p => p.Id == pId).SingleOrDefault();
            var topOffer = db.Auctions.Where(p => p.ProductId == pId).OrderByDescending(a => a.Price).FirstOrDefault();


            if (!(product.EndDate > DateTime.Now))
            {
                return RedirectToAction("AuctionByProduct", new { id = pId });
            }

            if (product.UserId == biderId)
            {
                // add the proper error/info message
                TempData["bidError"] = "your cannot bid on your own product.";
                return RedirectToAction("AuctionByProduct", new { id = pId });
            }
            if (bid > product.Price)
            {
                if (topOffer != null)
                {


                    if (bid - topOffer.Price > 5)
                    {
                        if (userBid != null)
                        {
                            userBid.Price = bid;
                            userBid.Date = DateTime.Now;
                            db.Entry(userBid).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            var newAuction = new Auction()
                            {
                                Price = bid,
                                ProductId = pId,
                                User = db.Users.Where(u => u.Id == biderId).SingleOrDefault(),
                                UserId = biderId,
                                Win = false,
                                Date = DateTime.Now
                            };
                            db.Auctions.Add(newAuction);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["bidError"] = "your offer should be 5 NOK higher than the top offer.";
                    }
                }
                else
                {
                    var newAuction = new Auction()
                    {
                        Price = bid,
                        ProductId = pId,
                        User = db.Users.Where(u => u.Id == biderId).SingleOrDefault(),
                        UserId = biderId,
                        Win = false,
                        Date = DateTime.Now
                    };
                    db.Auctions.Add(newAuction);
                    db.SaveChanges();
                }

            }
            else
            {
                TempData["bidError"] = "your offer should be higher than the product base price.";
            }

            return RedirectToAction("AuctionByProduct", new { id = pId });
        }


        public ActionResult AuctionsIWon()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var auctionsIWon = db.Sales.Include("Product").Where(a => a.UserId == userId && a.Payed == false).OrderByDescending(a => a.Date).ToList();
                return View(auctionsIWon);

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyAuctions()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var auctions = db.Auctions.Include("Product").Where(a => a.UserId == userId && a.Win == false).ToList();
                return View("Auctions", auctions);

            }
            return RedirectToAction("Index", "Home");

        }

        public JsonResult GetNumberOfMessages()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var noMessages = db.Messages.Where(m => m.UserRecId == userId && m.IsRead == false).Count();
                return Json(new { NumberOfMessages = noMessages }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetMessages(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var messages = db.Messages.Where(m => m.UserRecId == userId).OrderByDescending(m => m.Id).OrderBy(m => m.IsRead).ToList();
                if (messages != null)
                {

                    int take = 3;
                    var count = messages.Count();
                    int skip = 0;
                    if (count > take)
                    {
                        skip = (take * page) - take;
                    }

                    var TotalPages = Math.Ceiling((decimal)(count / take));
                    ViewBag.TotalPages = TotalPages;
                    var _page = page <= TotalPages ? page : TotalPages;
                    ViewBag.Page = _page;


                    return View(messages.Skip(skip).Take(take));
                }
            }
            return RedirectToAction("Index", "Manage");
        }


        public ActionResult ReadMessage(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id > 0)
                {
                    var userId = User.Identity.GetUserId();
                    var message = db.Messages.Where(m => m.Id == id).SingleOrDefault();

                    message.IsRead = true;
                    db.SaveChanges();
                    return View(message);
                }
            }
            return RedirectToAction("Index", "Manage");

        }


        public ActionResult Payment(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var itemsInCart = db.Sales.Where(s => s.UserId == userId && s.Id == id && s.Payed == false).SingleOrDefault();

                if (itemsInCart == null)
                {
                    return Content("Could not find the auction.");
                }
                // the gateway does not accepts amount less than 1000 Rials
                decimal totalPrice = 1000 + itemsInCart.Price;

                try
                {
                    MyEShop.Utilities.AuctionPayment.Payment ob = new MyEShop.Utilities.AuctionPayment.Payment();
                    string result = ob.pay(totalPrice.ToString());
                    JsonParameters parameters = JsonConvert.DeserializeObject<JsonParameters>(result);

                    if (parameters.status == 1)
                    {
                        itemsInCart.BankNo = parameters.transId;
                        db.SaveChanges();

                        Response.Redirect("https://pay.ir/payment/test/gateway/" + parameters.transId);
                    }
                    else
                    {
                        //lblresult.text = "کدخطا : " + parmeters.errorcode + "<br />" + "پیغام خطا : " + parmeters.errormessage;
                    }
                }
                catch (Exception e)
                {
                    //lblresult.text = "خطا در اتصال به درگاه پرداخت";
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult PaymentComplete(string transId)
        {


            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(transId))
                {
                    Payment ob = new Payment();
                    string result = ob.verify(transId.ToString());
                    JsonParameters Parmeters = JsonConvert.DeserializeObject<JsonParameters>(result);

                    if (Parmeters.status == 1)
                    {

                        var itemsInCart = db.Sales.Include("Product").Where(s => s.UserId == userId && s.BankNo == transId && s.Payed == false).ToList();

                        foreach (var item in itemsInCart)
                        {
                            item.Payed = true;
                            item.TransId = Convert.ToInt32(transId);
                        }
                        db.SaveChanges();

                        var lstId = new List<string>();
                        foreach (var item in itemsInCart)
                        {
                            lstId.Add(item.Product.UserId);
                        }


                        var bills = db.Bills.Where(b => lstId.Contains(b.UserId)).ToList();
                        var userBillId = bills.Select(b => b.UserId).ToList();


                        foreach (var v in itemsInCart)
                        {
                            Message messageToSeller = new Message()
                            {
                                Date = DateTime.Now,
                                IsRead = false,
                                Text = "You sold auction item " + v.Product.Name + " , Price: " + v.Price,
                                Title = "Auction Sold",
                                UserRecId = v.Product.UserId
                            };

                            db.Messages.Add(messageToSeller);

                            if (userBillId.Contains(v.Product.UserId))
                            {
                                var currentBill = db.Bills.Where(b => b.UserId == v.Product.UserId).SingleOrDefault();
                                currentBill.LastRec = Convert.ToInt32(v.Count * v.Price);
                                currentBill.TotalRec = currentBill.TotalRec + Convert.ToInt32(v.Count * v.Price);
                                currentBill.TimeOfLastRec = DateTime.Now;
                                db.SaveChanges();

                            }
                            else
                            {
                                Bill bill = new Bill()
                                {
                                    LastRec = Convert.ToInt32(v.Count * v.Price),
                                    TotalRec = Convert.ToInt32(v.Count * v.Price),
                                    TimeOfLastRec = DateTime.Now,
                                    PayMe = false,
                                    PayMeAmount = 0,
                                    UserId = v.Product.UserId,
                                    User = v.Product.User
                                };

                                db.Bills.Add(bill);
                            }

                        }
                        db.SaveChanges();



                        return RedirectToAction("GetSalesItemByTrasnId", "ShoppingCarts", new { tId = transId, price = Parmeters.amount.ToString() });
                    }
                    else
                    {
                        return Content("error");
                    }
                }
            }

            return RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = await db.Auctions.FindAsync(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId,UserId,Price,Win")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Auctions.Add(auction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", auction.UserId);
            return View(auction);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = await db.Auctions.FindAsync(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", auction.UserId);
            return View(auction);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId,UserId,Price,Win")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", auction.UserId);
            return View(auction);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = await db.Auctions.FindAsync(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Auction auction = await db.Auctions.FindAsync(id);
            db.Auctions.Remove(auction);
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
