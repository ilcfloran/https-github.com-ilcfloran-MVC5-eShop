namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserIdTypeInProcuct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
        }
    }
}
