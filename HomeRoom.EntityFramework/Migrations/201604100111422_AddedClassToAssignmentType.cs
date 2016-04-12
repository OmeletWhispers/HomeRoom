namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClassToAssignmentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssignmentTypes", "ClassId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssignmentTypes", "ClassId");
            AddForeignKey("dbo.AssignmentTypes", "ClassId", "dbo.Classes", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignmentTypes", "ClassId", "dbo.Classes");
            DropIndex("dbo.AssignmentTypes", new[] { "ClassId" });
            DropColumn("dbo.AssignmentTypes", "ClassId");
        }
    }
}
