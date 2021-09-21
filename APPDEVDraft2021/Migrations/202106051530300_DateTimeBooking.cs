namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeBooking : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookingTbls", "DateBooked", c => c.DateTime());
            AlterColumn("dbo.BookingTbls", "DateCheckedOut", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookingTbls", "DateCheckedOut", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BookingTbls", "DateBooked", c => c.DateTime(nullable: false));
        }
    }
}
