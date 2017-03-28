namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Email = c.String(),
                        Text = c.String(),
                        date = c.DateTime(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.ParentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ParentId);
            
            DropColumn("dbo.Products", "ProductImageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductImageId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ParentId", "dbo.Comments");
            DropIndex("dbo.Comments", new[] { "ParentId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Comments");
        }
    }
}
