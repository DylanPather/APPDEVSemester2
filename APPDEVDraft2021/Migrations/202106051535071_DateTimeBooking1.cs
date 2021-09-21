namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeBooking1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookingTbls", "DateCheckedIn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookingTbls", "DateCheckedIn", c => c.DateTime(nullable: false));
        }
    }
}
