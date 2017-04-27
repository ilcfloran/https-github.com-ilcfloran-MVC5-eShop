namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAvailableToBill : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bills", "Available");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "Available", c => c.Int(nullable: false));
        }
    }
}
