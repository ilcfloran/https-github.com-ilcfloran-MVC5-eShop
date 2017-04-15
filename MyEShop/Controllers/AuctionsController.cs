using Microsoft.AspNet.Identity;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using System;
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
            var topOffer = db.Auctions.OrderByDescending(a => a.Price).FirstOrDefault();


            if (!(product.EndDate > DateTime.Now))
            {
                return RedirectToAction("AuctionByProduct", new { id = pId });
            }

            if (product.UserId == biderId)
            {
                // add the proper error/info message
                return RedirectToAction("AuctionByProduct", new { id = pId });
            }

            if (topOffer != null)
            {
                if (bid - topOffer.Price > 5)
                {
                    if (userBid != null)
                    {
                        userBid.Price = bid;
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
                            Win = false
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
                    Win = false
                };
                db.Auctions.Add(newAuction);
                db.SaveChanges();
            }



            return RedirectToAction("AuctionByProduct", new { id = pId });
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
