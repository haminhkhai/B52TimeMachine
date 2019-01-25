namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restockServices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestockServices",
                c => new
                    {
                        RestockServiceId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Restock_RestockId = c.Int(),
                    })
                .PrimaryKey(t => t.RestockServiceId)
                .ForeignKey("dbo.Restocks", t => t.Restock_RestockId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.Restock_RestockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RestockServices", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.RestockServices", "Restock_RestockId", "dbo.Restocks");
            DropIndex("dbo.RestockServices", new[] { "Restock_RestockId" });
            DropIndex("dbo.RestockServices", new[] { "ServiceId" });
            DropTable("dbo.RestockServices");
        }
    }
}
