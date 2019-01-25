namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        WarehouseId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Service_ServiceId = c.Int(),
                    })
                .PrimaryKey(t => t.WarehouseId)
                .ForeignKey("dbo.Services", t => t.Service_ServiceId)
                .Index(t => t.Service_ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Warehouses", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.Warehouses", new[] { "Service_ServiceId" });
            DropTable("dbo.Warehouses");
        }
    }
}
