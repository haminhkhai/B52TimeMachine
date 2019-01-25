namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixServiceOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "Order", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "Order", c => c.Int(nullable: false));
        }
    }
}
