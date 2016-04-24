namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMessaging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        SentById = c.Long(nullable: false),
                        SentToId = c.Long(nullable: false),
                        DateRead = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.SentById, cascadeDelete: false)
                .ForeignKey("dbo.AbpUsers", t => t.SentToId, cascadeDelete: false)
                .Index(t => t.SentById)
                .Index(t => t.SentToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SentToId", "dbo.AbpUsers");
            DropForeignKey("dbo.Messages", "SentById", "dbo.AbpUsers");
            DropForeignKey("dbo.Announcements", "ClassId", "dbo.Classes");
            DropIndex("dbo.Messages", new[] { "SentToId" });
            DropIndex("dbo.Messages", new[] { "SentById" });
            DropIndex("dbo.Announcements", new[] { "ClassId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Announcements");
        }
    }
}
