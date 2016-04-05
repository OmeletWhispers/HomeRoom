namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeParentIdForeignKeyOptional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "ParentId", "dbo.Parents");
            DropIndex("dbo.Students", new[] { "ParentId" });
            AlterColumn("dbo.Students", "ParentId", c => c.Long());
            CreateIndex("dbo.Students", "ParentId");
            AddForeignKey("dbo.Students", "ParentId", "dbo.Parents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "ParentId", "dbo.Parents");
            DropIndex("dbo.Students", new[] { "ParentId" });
            AlterColumn("dbo.Students", "ParentId", c => c.Long(nullable: false));
            CreateIndex("dbo.Students", "ParentId");
            AddForeignKey("dbo.Students", "ParentId", "dbo.Parents", "Id", cascadeDelete: true);
        }
    }
}
