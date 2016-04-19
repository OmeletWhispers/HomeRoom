namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTheTestGenerator : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerChoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.AssignmentAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignmentId = c.Int(nullable: false),
                        StudentId = c.Long(nullable: false),
                        AnswerChoiceId = c.Int(),
                        Text = c.String(),
                        EarnedPoints = c.Int(nullable: false),
                        AnswerChoices_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnswerChoices", t => t.AnswerChoices_Id)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.AssignmentId)
                .Index(t => t.StudentId)
                .Index(t => t.AnswerChoices_Id);
            
            CreateTable(
                "dbo.AssignmentQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        AssignmentId = c.Int(nullable: false),
                        PointValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        QuestionType = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TeacherId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: false)
                .Index(t => t.TeacherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.AssignmentAnswers", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Categories", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Questions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.AssignmentQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AnswerChoices", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AssignmentQuestions", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.AssignmentAnswers", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.AssignmentAnswers", "AnswerChoices_Id", "dbo.AnswerChoices");
            DropIndex("dbo.Subjects", new[] { "TeacherId" });
            DropIndex("dbo.Categories", new[] { "SubjectId" });
            DropIndex("dbo.Questions", new[] { "CategoryId" });
            DropIndex("dbo.AssignmentQuestions", new[] { "AssignmentId" });
            DropIndex("dbo.AssignmentQuestions", new[] { "QuestionId" });
            DropIndex("dbo.AssignmentAnswers", new[] { "AnswerChoices_Id" });
            DropIndex("dbo.AssignmentAnswers", new[] { "StudentId" });
            DropIndex("dbo.AssignmentAnswers", new[] { "AssignmentId" });
            DropIndex("dbo.AnswerChoices", new[] { "QuestionId" });
            DropTable("dbo.Subjects");
            DropTable("dbo.Categories");
            DropTable("dbo.Questions");
            DropTable("dbo.AssignmentQuestions");
            DropTable("dbo.AssignmentAnswers");
            DropTable("dbo.AnswerChoices");
        }
    }
}
