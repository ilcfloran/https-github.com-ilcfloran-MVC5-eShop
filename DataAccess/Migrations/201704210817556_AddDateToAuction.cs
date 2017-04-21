namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateToAuction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Auctions", "Date");
        }
    }
}
