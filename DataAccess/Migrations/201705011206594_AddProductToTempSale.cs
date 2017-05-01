namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductToTempSale : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TempSales", "ProductId");
            AddForeignKey("dbo.TempSales", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempSales", "ProductId", "dbo.Products");
            DropIndex("dbo.TempSales", new[] { "ProductId" });
        }
    }
}
