namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "PayMe", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "PayMeAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "PayMeAmount");
            DropColumn("dbo.Bills", "PayMe");
        }
    }
}
