namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPayment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupNo = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransNo = c.String(maxLength: 15),
                        BankGetNo = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.GroupNo, name: "IX_Payment_GroupNo");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payments", "IX_Payment_GroupNo");
            DropTable("dbo.Payments");
        }
    }
}
