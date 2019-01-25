namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PsFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ps", "IsVisible", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Ps", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ps", "Name", c => c.String());
            DropColumn("dbo.Ps", "IsVisible");
        }
    }
}
