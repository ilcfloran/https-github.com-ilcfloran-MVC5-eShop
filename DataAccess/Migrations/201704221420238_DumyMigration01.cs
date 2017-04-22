namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DumyMigration01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "peda", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "peda");
        }
    }
}
