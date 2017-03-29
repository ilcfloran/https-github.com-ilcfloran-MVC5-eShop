namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationBetweenCategoryAndGroupFilter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories_GroupFilters",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        GroupFilterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.GroupFilterId })
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.GroupFilters", t => t.GroupFilterId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.GroupFilterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories_GroupFilters", "GroupFilterId", "dbo.GroupFilters");
            DropForeignKey("dbo.Categories_GroupFilters", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Categories_GroupFilters", new[] { "GroupFilterId" });
            DropIndex("dbo.Categories_GroupFilters", new[] { "CategoryId" });
            DropTable("dbo.Categories_GroupFilters");
        }
    }
}
