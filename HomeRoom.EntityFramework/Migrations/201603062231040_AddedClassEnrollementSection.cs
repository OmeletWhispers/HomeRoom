namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClassEnrollementSection : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Teachers", new[] { "UserId" });
            DropColumn("dbo.Teachers", "Id");
            RenameColumn(table: "dbo.Parents", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.Students", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.Teachers", name: "UserId", newName: "Id");
            RenameIndex(table: "dbo.Students", name: "IX_UserId", newName: "IX_Id");
            RenameIndex(table: "dbo.Parents", name: "IX_UserId", newName: "IX_Id");
            DropPrimaryKey("dbo.Teachers");
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Subject = c.String(),
                        TeacherId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        StudentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ClassId)
                .Index(t => t.StudentId);
            
            AlterColumn("dbo.Teachers", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Teachers", "Id");
            CreateIndex("dbo.Teachers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Classes", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Enrollments", "ClassId", "dbo.Classes");
            DropIndex("dbo.Teachers", new[] { "Id" });
            DropIndex("dbo.Enrollments", new[] { "StudentId" });
            DropIndex("dbo.Enrollments", new[] { "ClassId" });
            DropIndex("dbo.Classes", new[] { "TeacherId" });
            DropPrimaryKey("dbo.Teachers");
            AlterColumn("dbo.Teachers", "Id", c => c.Int(nullable: false));
            DropTable("dbo.Enrollments");
            DropTable("dbo.Classes");
            AddPrimaryKey("dbo.Teachers", "UserId");
            RenameIndex(table: "dbo.Parents", name: "IX_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Students", name: "IX_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Teachers", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Students", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Parents", name: "Id", newName: "UserId");
            AddColumn("dbo.Teachers", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Teachers", "UserId");
        }
    }
}
