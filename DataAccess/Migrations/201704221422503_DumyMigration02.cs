namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DumyMigration02 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "peda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "peda", c => c.Int(nullable: false));
        }
    }
}
