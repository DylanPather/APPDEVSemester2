namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportCarts",
                c => new
                    {
                        ReportCartID = c.Int(nullable: false, identity: true),
                        CartID = c.String(),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportCartID)
                .ForeignKey("dbo.StockServiceTbls", t => t.StockID, cascadeDelete: true)
                .Index(t => t.StockID);
            
            CreateTable(
                "dbo.ReportDetailTbls",
                c => new
                    {
                        ReportDetailID = c.Int(nullable: false, identity: true),
                        ReportID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportDetailID);
            
            CreateTable(
                "dbo.ReportTbls",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        BayID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        VehicleRegNo = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportCarts", "StockID", "dbo.StockServiceTbls");
            DropIndex("dbo.ReportCarts", new[] { "StockID" });
            DropTable("dbo.ReportTbls");
            DropTable("dbo.ReportDetailTbls");
            DropTable("dbo.ReportCarts");
        }
    }
}
