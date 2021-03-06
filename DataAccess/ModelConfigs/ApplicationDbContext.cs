﻿using Microsoft.AspNet.Identity.EntityFramework;
using MyEShop.Core.Models;
using System.Data.Entity;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        //DbSet s goes here
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<TempSale> TempSale { get; set; }
        public DbSet<GroupFilter> GroupFilter { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<FilterItem> FilterItems { get; set; }
        public DbSet<CategoriesGroupFilters> CategoriesGroupFilters { get; set; }
        //public DbSet<ProductsFilterItems> ProductsFilterItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Bill> Bills { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ProductImageConfiguration());
            modelBuilder.Configurations.Add(new AuctionConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new SaleConfiguration());
            modelBuilder.Configurations.Add(new PaymentConfiguration());
            modelBuilder.Configurations.Add(new FilterItemConfiguration());
            modelBuilder.Configurations.Add(new GroupFilterConfiguration());
            modelBuilder.Configurations.Add(new CategoriesGroupFiltersConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            //modelBuilder.Configurations.Add(new ProductsFilterItemsConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());


            base.OnModelCreating(modelBuilder);
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
