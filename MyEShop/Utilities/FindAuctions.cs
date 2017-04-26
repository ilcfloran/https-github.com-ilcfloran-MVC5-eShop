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

            var auctions = db.Products.Where(p => p.EndDate != null);

            foreach (var item in auctions.Where(a => a.EndDate < DateTime.Now))
            {
                var AuctionWithMaxPrice = db.Auctions.Where(au => au.ProductId == item.Id).OrderByDescending(a => a.Price).SingleOrDefault();
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
            }

        }

    }
}