namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcutFiltersRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products_FilterItems",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        FilterItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.FilterItemId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.FilterItems", t => t.FilterItemId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.FilterItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products_FilterItems", "FilterItemId", "dbo.FilterItems");
            DropForeignKey("dbo.Products_FilterItems", "ProductId", "dbo.Products");
            DropIndex("dbo.Products_FilterItems", new[] { "FilterItemId" });
            DropIndex("dbo.Products_FilterItems", new[] { "ProductId" });
            DropTable("dbo.Products_FilterItems");
        }
    }
}
