namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statusinreport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportTbls", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportTbls", "Status");
        }
    }
}
