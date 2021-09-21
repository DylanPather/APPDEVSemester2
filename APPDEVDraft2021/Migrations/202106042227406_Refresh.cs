namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refresh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingTbls", "DateCheckedIn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingTbls", "DateCheckedIn");
        }
    }
}
