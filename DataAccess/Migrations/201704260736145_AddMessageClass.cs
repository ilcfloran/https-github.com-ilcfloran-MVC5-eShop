namespace MyEShop.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserRecId = c.String(nullable: false, maxLength: 128),
                        UserSendId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 500),
                        Text = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserRecId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserSendId)
                .Index(t => t.UserRecId)
                .Index(t => t.UserSendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserSendId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "UserRecId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "UserSendId" });
            DropIndex("dbo.Messages", new[] { "UserRecId" });
            DropTable("dbo.Messages");
        }
    }
}
