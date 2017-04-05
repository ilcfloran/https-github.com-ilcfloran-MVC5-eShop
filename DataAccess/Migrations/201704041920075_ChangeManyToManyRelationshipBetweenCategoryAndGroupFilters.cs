namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeManyToManyRelationshipBetweenCategoryAndGroupFilters : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories_GroupFilters", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories_GroupFilters", "GroupFilterId", "dbo.GroupFilters");
            DropIndex("dbo.Categories_GroupFilters", new[] { "CategoryId" });
            DropIndex("dbo.Categories_GroupFilters", new[] { "GroupFilterId" });
            DropTable("dbo.Categories_GroupFilters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Categories_GroupFilters",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        GroupFilterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.GroupFilterId });
            
            CreateIndex("dbo.Categories_GroupFilters", "GroupFilterId");
            CreateIndex("dbo.Categories_GroupFilters", "CategoryId");
            AddForeignKey("dbo.Categories_GroupFilters", "GroupFilterId", "dbo.GroupFilters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Categories_GroupFilters", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
    }
}
