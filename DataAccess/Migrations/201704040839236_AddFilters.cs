namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupFilters", "FilterItem_Id", c => c.Int());
            AddColumn("dbo.FilterItems", "ParentFilterItemId", c => c.Int());
            AlterColumn("dbo.GroupFilters", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.FilterItems", "Title", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.GroupFilters", "FilterItem_Id");
            CreateIndex("dbo.FilterItems", "Title", name: "AK_FilterItem_Title");
            CreateIndex("dbo.FilterItems", "ParentFilterItemId");
            AddForeignKey("dbo.GroupFilters", "FilterItem_Id", "dbo.FilterItems", "Id");
            AddForeignKey("dbo.FilterItems", "ParentFilterItemId", "dbo.FilterItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilterItems", "ParentFilterItemId", "dbo.FilterItems");
            DropForeignKey("dbo.GroupFilters", "FilterItem_Id", "dbo.FilterItems");
            DropIndex("dbo.FilterItems", new[] { "ParentFilterItemId" });
            DropIndex("dbo.FilterItems", "AK_FilterItem_Title");
            DropIndex("dbo.GroupFilters", new[] { "FilterItem_Id" });
            AlterColumn("dbo.FilterItems", "Title", c => c.String());
            AlterColumn("dbo.GroupFilters", "Title", c => c.String());
            DropColumn("dbo.FilterItems", "ParentFilterItemId");
            DropColumn("dbo.GroupFilters", "FilterItem_Id");
        }
    }
}
