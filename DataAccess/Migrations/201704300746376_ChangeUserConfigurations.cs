namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserConfigurations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 20));
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 150));
        }
    }
}
