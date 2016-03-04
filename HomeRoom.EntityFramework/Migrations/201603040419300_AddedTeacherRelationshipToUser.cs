namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTeacherRelationshipToUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AbpUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AbpUsers", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.Teachers", new[] { "UserId" });
            DropColumn("dbo.AbpUsers", "Gender");
            DropTable("dbo.Teachers");
        }
    }
}
