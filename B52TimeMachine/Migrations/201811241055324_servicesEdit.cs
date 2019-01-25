namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicesEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Services", "IsCig", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "IsVisible", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Services", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "Name", c => c.String());
            DropColumn("dbo.Services", "IsVisible");
            DropColumn("dbo.Services", "IsCig");
            DropColumn("dbo.Services", "Order");
        }
    }
}
