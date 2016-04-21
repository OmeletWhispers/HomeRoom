namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedValueColumnForQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Value");
        }
    }
}
