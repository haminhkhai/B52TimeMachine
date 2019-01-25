namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PsAvailabl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ps", "IsAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ps", "IsAvailable");
        }
    }
}
