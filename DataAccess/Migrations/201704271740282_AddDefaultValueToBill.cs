namespace MyEShop.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDefaultValueToBill : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "PayMeAmount", c => c.Int(nullable: false, defaultValue: 0));

            AlterColumn("dbo.Bills", "PayMe", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
        }
    }
}
