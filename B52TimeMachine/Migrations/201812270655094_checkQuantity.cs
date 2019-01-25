namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkQuantity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckQuantities",
                c => new
                    {
                        CheckQuantityId = c.Int(nullable: false, identity: true),
                        Margin = c.Int(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        CheckDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CheckQuantityId);
            
            CreateTable(
                "dbo.CheckQuantityDetails",
                c => new
                    {
                        CheckQuantityDetailId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        OldQuantity = c.Int(nullable: false),
                        CurrentQuantity = c.Int(nullable: false),
                        Margin = c.Int(nullable: false),
                        CheckQuantity_CheckQuantityId = c.Int(),
                    })
                .PrimaryKey(t => t.CheckQuantityDetailId)
                .ForeignKey("dbo.CheckQuantities", t => t.CheckQuantity_CheckQuantityId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.CheckQuantity_CheckQuantityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckQuantityDetails", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.CheckQuantityDetails", "CheckQuantity_CheckQuantityId", "dbo.CheckQuantities");
            DropIndex("dbo.CheckQuantityDetails", new[] { "CheckQuantity_CheckQuantityId" });
            DropIndex("dbo.CheckQuantityDetails", new[] { "ServiceId" });
            DropTable("dbo.CheckQuantityDetails");
            DropTable("dbo.CheckQuantities");
        }
    }
}
