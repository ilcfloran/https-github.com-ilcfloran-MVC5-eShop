namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToAuction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "UserId" });
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        GroupNo = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        Payed = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId, name: "IX_Sale_ProductId")
                .Index(t => t.UserId);
            
            AlterColumn("dbo.Auctions", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "Text", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.ProductImages", "ImageName", c => c.String(nullable: false, maxLength: 80));
            CreateIndex("dbo.Auctions", new[] { "UserId", "ProductId" }, unique: true, name: "IX_Auction");
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Comments", "ProductId", name: "IX_Comment_ProductId");
            AddForeignKey("dbo.Auctions", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sales", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sales", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Auctions", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Sales", new[] { "UserId" });
            DropIndex("dbo.Sales", "IX_Sale_ProductId");
            DropIndex("dbo.Comments", "IX_Comment_ProductId");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Auctions", "IX_Auction");
            AlterColumn("dbo.ProductImages", "ImageName", c => c.String());
            AlterColumn("dbo.Comments", "Text", c => c.String());
            AlterColumn("dbo.Comments", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Auctions", "UserId", c => c.String());
            DropTable("dbo.Sales");
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
