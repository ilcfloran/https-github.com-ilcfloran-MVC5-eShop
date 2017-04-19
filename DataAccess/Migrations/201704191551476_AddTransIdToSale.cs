namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransIdToSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sales", "TransId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sales", "TransId");
        }
    }
}
