namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuestionToAssignmentAnswers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AssignmentAnswers", "AnswerChoiceId");
            RenameColumn(table: "dbo.AssignmentAnswers", name: "AnswerChoices_Id", newName: "AnswerChoiceId");
            RenameIndex(table: "dbo.AssignmentAnswers", name: "IX_AnswerChoices_Id", newName: "IX_AnswerChoiceId");
            AddColumn("dbo.AssignmentAnswers", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssignmentAnswers", "QuestionId");
            AddForeignKey("dbo.AssignmentAnswers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignmentAnswers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.AssignmentAnswers", new[] { "QuestionId" });
            DropColumn("dbo.AssignmentAnswers", "QuestionId");
            RenameIndex(table: "dbo.AssignmentAnswers", name: "IX_AnswerChoiceId", newName: "IX_AnswerChoices_Id");
            RenameColumn(table: "dbo.AssignmentAnswers", name: "AnswerChoiceId", newName: "AnswerChoices_Id");
            AddColumn("dbo.AssignmentAnswers", "AnswerChoiceId", c => c.Int());
        }
    }
}
