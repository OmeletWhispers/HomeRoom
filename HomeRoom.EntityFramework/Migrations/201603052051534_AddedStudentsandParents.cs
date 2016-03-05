namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStudentsandParents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AbpUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        ParentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AbpUsers", t => t.UserId)
                .ForeignKey("dbo.Parents", t => t.ParentId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "ParentId", "dbo.Parents");
            DropForeignKey("dbo.Students", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.Parents", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.Students", new[] { "ParentId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.Parents", new[] { "UserId" });
            DropTable("dbo.Students");
            DropTable("dbo.Parents");
        }
    }
}
