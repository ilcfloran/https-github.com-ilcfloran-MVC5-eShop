namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilterItemCreateRelationBetweenGroupFilterAndFilterItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilterItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        GroupFilterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupFilters", t => t.GroupFilterId, cascadeDelete: true)
                .Index(t => t.GroupFilterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilterItems", "GroupFilterId", "dbo.GroupFilters");
            DropIndex("dbo.FilterItems", new[] { "GroupFilterId" });
            DropTable("dbo.FilterItems");
        }
    }
}
