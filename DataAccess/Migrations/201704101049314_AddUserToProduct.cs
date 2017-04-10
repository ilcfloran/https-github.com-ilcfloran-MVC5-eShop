namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "UserId");
            AddForeignKey("dbo.Products", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "UserId" });
            AlterColumn("dbo.Products", "UserId", c => c.String());
        }
    }
}
