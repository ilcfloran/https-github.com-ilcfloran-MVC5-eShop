namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductToAuction : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Auctions", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "ProductId", "dbo.Products");
        }
    }
}
