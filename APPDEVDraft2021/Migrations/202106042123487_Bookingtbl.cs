namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bookingtbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingTbls", "Status", c => c.String());
            AddColumn("dbo.BookingTbls", "CheckIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.BookingTbls", "CheckOut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingTbls", "CheckOut");
            DropColumn("dbo.BookingTbls", "CheckIn");
            DropColumn("dbo.BookingTbls", "Status");
        }
    }
}
