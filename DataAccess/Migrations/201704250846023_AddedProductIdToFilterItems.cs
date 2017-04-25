namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductIdToFilterItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilterItems", "ProductId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FilterItems", "ProductId");
        }
    }
}
