namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceRental : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceSolds", "RentalId", "dbo.Rentals");
            DropForeignKey("dbo.ServiceSolds", "ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceSolds", new[] { "ServiceId" });
            DropIndex("dbo.ServiceSolds", new[] { "RentalId" });
            CreateTable(
                "dbo.ServiceRentals",
                c => new
                    {
                        ServiceRentalId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        RentalId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceRentalId)
                .ForeignKey("dbo.Rentals", t => t.RentalId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.RentalId);
            
            DropTable("dbo.ServiceSolds");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceSolds",
                c => new
                    {
                        ServiceSoldId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        RentalId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceSoldId);
            
            DropForeignKey("dbo.ServiceRentals", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceRentals", "RentalId", "dbo.Rentals");
            DropIndex("dbo.ServiceRentals", new[] { "RentalId" });
            DropIndex("dbo.ServiceRentals", new[] { "ServiceId" });
            DropTable("dbo.ServiceRentals");
            CreateIndex("dbo.ServiceSolds", "RentalId");
            CreateIndex("dbo.ServiceSolds", "ServiceId");
            AddForeignKey("dbo.ServiceSolds", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
            AddForeignKey("dbo.ServiceSolds", "RentalId", "dbo.Rentals", "RentalId", cascadeDelete: true);
        }
    }
}
