namespace HomeRoom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountTypeToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "AccountType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "AccountType");
        }
    }
}
