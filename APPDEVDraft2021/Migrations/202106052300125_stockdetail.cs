namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockdetail : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.QuoteDetailTbls", "StockID");
            AddForeignKey("dbo.QuoteDetailTbls", "StockID", "dbo.StockServiceTbls", "StockID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuoteDetailTbls", "StockID", "dbo.StockServiceTbls");
            DropIndex("dbo.QuoteDetailTbls", new[] { "StockID" });
        }
    }
}
