namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuotationTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuoteCarts",
                c => new
                    {
                        QuoteCartID = c.Int(nullable: false, identity: true),
                        CartID = c.String(),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuoteCartID)
                .ForeignKey("dbo.StockServiceTbls", t => t.StockID, cascadeDelete: true)
                .Index(t => t.StockID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuoteCarts", "StockID", "dbo.StockServiceTbls");
            DropIndex("dbo.QuoteCarts", new[] { "StockID" });
            DropTable("dbo.QuoteCarts");
        }
    }
}
