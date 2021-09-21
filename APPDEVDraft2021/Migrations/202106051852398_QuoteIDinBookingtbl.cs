namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuoteIDinBookingtbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingTbls", "QuotationID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingTbls", "QuotationID");
        }
    }
}
