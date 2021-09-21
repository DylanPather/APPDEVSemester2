namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuotationTbls",
                c => new
                    {
                        QuotationID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        QuoteDate = c.DateTime(nullable: false),
                        QuoteTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateModified = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.QuotationID);
            
            CreateTable(
                "dbo.QuoteDetailTbls",
                c => new
                    {
                        QuoteDetailID = c.Int(nullable: false, identity: true),
                        QuoteID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        QuotationTbl_QuotationID = c.Int(),
                    })
                .PrimaryKey(t => t.QuoteDetailID)
                .ForeignKey("dbo.QuotationTbls", t => t.QuotationTbl_QuotationID)
                .Index(t => t.QuotationTbl_QuotationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuoteDetailTbls", "QuotationTbl_QuotationID", "dbo.QuotationTbls");
            DropIndex("dbo.QuoteDetailTbls", new[] { "QuotationTbl_QuotationID" });
            DropTable("dbo.QuoteDetailTbls");
            DropTable("dbo.QuotationTbls");
        }
    }
}
