namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBillClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Available = c.Int(nullable: false),
                        Withdrawable = c.Int(nullable: false),
                        LastRec = c.Int(nullable: false),
                        TotalRec = c.Int(nullable: false),
                        TimeOfLastRec = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bills");
        }
    }
}
