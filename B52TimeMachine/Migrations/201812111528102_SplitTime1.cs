namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitTime1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SplitTimes",
                c => new
                    {
                        SplitTimeId = c.Int(nullable: false, identity: true),
                        TimeSplit = c.DateTime(nullable: false),
                        Rental_RentalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SplitTimeId)
                .ForeignKey("dbo.Rentals", t => t.Rental_RentalId, cascadeDelete: true)
                .Index(t => t.Rental_RentalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SplitTimes", "Rental_RentalId", "dbo.Rentals");
            DropIndex("dbo.SplitTimes", new[] { "Rental_RentalId" });
            DropTable("dbo.SplitTimes");
        }
    }
}
