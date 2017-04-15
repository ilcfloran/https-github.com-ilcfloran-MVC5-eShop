namespace MyEShop.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDefaultValueToCountInShoppingCart : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShoppingCarts", "Count", c => c.Int(nullable: false, defaultValue: 1));

        }

        public override void Down()
        {
        }
    }
}
