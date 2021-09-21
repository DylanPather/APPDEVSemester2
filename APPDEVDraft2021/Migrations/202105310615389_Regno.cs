namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerVehicles", "VehicleRegNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerVehicles", "VehicleRegNo");
        }
    }
}
