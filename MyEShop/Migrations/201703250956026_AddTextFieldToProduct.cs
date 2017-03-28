namespace MyEShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTextFieldToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Text", c => c.String(maxLength: 100));
            AlterColumn("dbo.Products", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Products", "Text");
        }
    }
}
