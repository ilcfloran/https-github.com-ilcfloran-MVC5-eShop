namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBankInfoToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BankName", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankAcount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BankAcount");
            DropColumn("dbo.AspNetUsers", "BankName");
        }
    }
}
