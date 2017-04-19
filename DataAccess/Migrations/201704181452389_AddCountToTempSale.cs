namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountToTempSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempSales", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempSales", "Count");
        }
    }
}
