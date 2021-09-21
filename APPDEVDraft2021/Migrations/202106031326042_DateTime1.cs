namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTime1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.QuotationTbls", "CustomerID");
            AddForeignKey("dbo.QuotationTbls", "CustomerID", "dbo.CustomerTbls", "CustomerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuotationTbls", "CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.QuotationTbls", new[] { "CustomerID" });
        }
    }
}
