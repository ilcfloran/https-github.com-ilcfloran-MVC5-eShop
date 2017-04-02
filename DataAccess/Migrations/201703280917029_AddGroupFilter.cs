namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupFilter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupFilters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupFilters", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.TempSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShoppingCartId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        BankGetNo = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupFilters", "ParentId", "dbo.GroupFilters");
            DropIndex("dbo.GroupFilters", new[] { "ParentId" });
            DropTable("dbo.TempSales");
            DropTable("dbo.GroupFilters");
        }
    }
}
