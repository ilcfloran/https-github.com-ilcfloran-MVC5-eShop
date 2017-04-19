namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateChanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempSales", "ProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempSales", "ProductName");
        }
    }
}
