namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartidforReporttbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportTbls", "CartID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportTbls", "CartID");
        }
    }
}
