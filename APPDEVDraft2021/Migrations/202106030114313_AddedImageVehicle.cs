namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerVehicles", "VehicleImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerVehicles", "VehicleImage");
        }
    }
}
