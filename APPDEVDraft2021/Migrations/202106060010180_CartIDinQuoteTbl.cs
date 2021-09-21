namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartIDinQuoteTbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuotationTbls", "CartID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuotationTbls", "CartID");
        }
    }
}
