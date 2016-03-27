namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGradeBookSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignmentTypeId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssignmentTypes", t => t.AssignmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.AssignmentTypeId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.AssignmentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Percentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExtraCredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        ClassId = c.Int(nullable: false),
                        Name = c.String(),
                        Points = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignmentId = c.Int(nullable: false),
                        StudentId = c.Long(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.AssignmentId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grades", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Grades", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.ExtraCredits", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ExtraCredits", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Assignments", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Assignments", "AssignmentTypeId", "dbo.AssignmentTypes");
            DropIndex("dbo.Grades", new[] { "StudentId" });
            DropIndex("dbo.Grades", new[] { "AssignmentId" });
            DropIndex("dbo.ExtraCredits", new[] { "ClassId" });
            DropIndex("dbo.ExtraCredits", new[] { "StudentId" });
            DropIndex("dbo.Assignments", new[] { "ClassId" });
            DropIndex("dbo.Assignments", new[] { "AssignmentTypeId" });
            DropTable("dbo.Grades");
            DropTable("dbo.ExtraCredits");
            DropTable("dbo.AssignmentTypes");
            DropTable("dbo.Assignments");
        }
    }
}
