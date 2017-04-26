using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using System;
using System.Linq;

namespace MyEShop.Utilities
{
    public static class FindAuctions
    {
        public static void FindEndedAuction()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var auctionProducts = db.Products.Where(p => p.EndDate != null);

            foreach (var item in auctionProducts.Where(a => a.EndDate < DateTime.Now))
            {
                var allAuctionsForTheProduct = db.Auctions.Where(au => au.ProductId == item.Id).ToList();
                var AuctionWithMaxPrice = allAuctionsForTheProduct.OrderByDescending(a => a.Price).FirstOrDefault();
                if (AuctionWithMaxPrice != null)
                {
                    Sale sales = new Sale()
                    {
                        TrackingCode = 0,
                        Count = 1,
                        Date = item.EndDate ?? DateTime.Now,
                        Payed = false,
                        Price = AuctionWithMaxPrice.Price,
                        StatusId = SalesStatus.OnHold,
                        TransId = 0,
                        UserId = AuctionWithMaxPrice.UserId,
                        ProductId = AuctionWithMaxPrice.ProductId,
                        Product = AuctionWithMaxPrice.Product
                    };
                    //AuctionWithMaxPrice.Win = true;
                    db.Sales.Add(sales);
                    //db.SaveChanges();

                    foreach (var x in allAuctionsForTheProduct.ToList())
                    {
                        Message message = new Message();
                        if (AuctionWithMaxPrice.Id == x.Id)
                        {
                            message.Text = "You won the auction for " + x.Product.Name + " ,Please proceed to payment.";
                        }
                        else
                        {
                            message.Text = "You did not win the auction for " + x.Product.Name;

                        }

                        message.Date = DateTime.Now;
                        message.IsRead = false;
                        message.UserRecId = x.UserId;
                        message.Title = "Auction result for " + x.Product.Name;

                        db.Messages.Add(message);
                        db.Auctions.Remove(x);
                    }
                }
            }
            db.SaveChanges();
            auctionProducts = null;
            db.Dispose();
        }
    }
}