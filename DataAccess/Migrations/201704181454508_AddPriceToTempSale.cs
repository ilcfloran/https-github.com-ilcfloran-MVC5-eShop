namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceToTempSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempSales", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempSales", "Price");
        }
    }
}
