namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBillClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bills", "UserId");
            AddForeignKey("dbo.Bills", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Bills", "Available");
            DropColumn("dbo.Bills", "Withdrawable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "Withdrawable", c => c.Int(nullable: false));
            AddColumn("dbo.Bills", "Available", c => c.Int(nullable: false));
            DropForeignKey("dbo.Bills", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bills", new[] { "UserId" });
            AlterColumn("dbo.Bills", "UserId", c => c.String());
        }
    }
}
