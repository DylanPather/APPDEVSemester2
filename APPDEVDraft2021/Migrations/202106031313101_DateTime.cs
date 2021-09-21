namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuotationTbls", "QuoteDate", c => c.DateTime());
            AlterColumn("dbo.QuotationTbls", "DateModified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuotationTbls", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QuotationTbls", "QuoteDate", c => c.DateTime(nullable: false));
        }
    }
}
