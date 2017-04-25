namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temp03 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.FilterItemProducts");
            AddPrimaryKey("dbo.FilterItemProducts", new[] { "Product_Id", "FilterItem_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.FilterItemProducts");
            AddPrimaryKey("dbo.FilterItemProducts", new[] { "FilterItem_Id", "Product_Id" });
        }
    }
}
