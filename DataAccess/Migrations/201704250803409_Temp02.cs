namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temp02 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Products_FilterItems", newName: "FilterItemProducts");
            RenameColumn(table: "dbo.FilterItemProducts", name: "ProductId", newName: "Product_Id");
            RenameColumn(table: "dbo.FilterItemProducts", name: "FilterItemId", newName: "FilterItem_Id");
            RenameIndex(table: "dbo.FilterItemProducts", name: "IX_FilterItemId", newName: "IX_FilterItem_Id");
            RenameIndex(table: "dbo.FilterItemProducts", name: "IX_ProductId", newName: "IX_Product_Id");
            DropPrimaryKey("dbo.FilterItemProducts");
            AddPrimaryKey("dbo.FilterItemProducts", new[] { "FilterItem_Id", "Product_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.FilterItemProducts");
            AddPrimaryKey("dbo.FilterItemProducts", new[] { "ProductId", "FilterItemId" });
            RenameIndex(table: "dbo.FilterItemProducts", name: "IX_Product_Id", newName: "IX_ProductId");
            RenameIndex(table: "dbo.FilterItemProducts", name: "IX_FilterItem_Id", newName: "IX_FilterItemId");
            RenameColumn(table: "dbo.FilterItemProducts", name: "FilterItem_Id", newName: "FilterItemId");
            RenameColumn(table: "dbo.FilterItemProducts", name: "Product_Id", newName: "ProductId");
            RenameTable(name: "dbo.FilterItemProducts", newName: "Products_FilterItems");
        }
    }
}
