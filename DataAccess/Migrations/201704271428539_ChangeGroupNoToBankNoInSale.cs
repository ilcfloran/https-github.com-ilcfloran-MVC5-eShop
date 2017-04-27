namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGroupNoToBankNoInSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sales", "BankNo", c => c.String());
            DropColumn("dbo.Sales", "GroupNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "GroupNo", c => c.Int(nullable: false));
            DropColumn("dbo.Sales", "BankNo");
        }
    }
}
