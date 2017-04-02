namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductCategoryModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        ParentCategoryId = c.Int(),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Categories", t => t.ParentCategoryId)
                .Index(t => t.ParentCategoryId)
                .Index(t => t.CategoryName, unique: true, name: "AK_Category_CategoryName");
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 70),
                        Description = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Visit = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        weight = c.Int(),
                        Onsale = c.Boolean(nullable: false),
                        OnSalePrice = c.Decimal(precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CategoryId, name: "IX_Product_CategoryId");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories");
            DropIndex("dbo.Products", "IX_Product_CategoryId");
            DropIndex("dbo.Categories", "AK_Category_CategoryName");
            DropIndex("dbo.Categories", new[] { "ParentCategoryId" });
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
