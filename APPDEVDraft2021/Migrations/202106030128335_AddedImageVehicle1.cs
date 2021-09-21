namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageVehicle1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RecordQuotes", "VehicleID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecordQuotes", "VehicleID", c => c.String());
        }
    }
}
