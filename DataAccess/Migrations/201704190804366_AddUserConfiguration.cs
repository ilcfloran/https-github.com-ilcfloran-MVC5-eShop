namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserConfiguration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 150));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "State", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 20));
            AlterColumn("dbo.AspNetUsers", "Mobile", c => c.String(maxLength: 15));
            AlterColumn("dbo.AspNetUsers", "BankName", c => c.String(maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "BankAcount", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "BankAcount", c => c.String());
            AlterColumn("dbo.AspNetUsers", "BankName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Mobile", c => c.String());
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String());
            AlterColumn("dbo.AspNetUsers", "State", c => c.String());
            AlterColumn("dbo.AspNetUsers", "City", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String());
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
        }
    }
}
