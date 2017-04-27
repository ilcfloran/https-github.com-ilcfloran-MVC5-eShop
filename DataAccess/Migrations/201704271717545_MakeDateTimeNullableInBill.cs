namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDateTimeNullableInBill : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "TimeOfLastRec", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bills", "TimeOfLastRec", c => c.DateTime(nullable: false));
        }
    }
}
