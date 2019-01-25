namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Restocks",
                c => new
                    {
                        RestockId = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        RestockDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RestockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Restocks");
        }
    }
}
