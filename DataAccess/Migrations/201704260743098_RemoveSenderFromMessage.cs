namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSenderFromMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "UserSendId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "UserSendId" });
            DropColumn("dbo.Messages", "UserSendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "UserSendId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Messages", "UserSendId");
            AddForeignKey("dbo.Messages", "UserSendId", "dbo.AspNetUsers", "Id");
        }
    }
}
