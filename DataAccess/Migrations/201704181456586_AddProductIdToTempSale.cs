namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductIdToTempSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempSales", "ProductId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempSales", "ProductId");
        }
    }
}
