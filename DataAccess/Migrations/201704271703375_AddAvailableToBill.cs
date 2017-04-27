namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailableToBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "Available", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "Available");
        }
    }
}
