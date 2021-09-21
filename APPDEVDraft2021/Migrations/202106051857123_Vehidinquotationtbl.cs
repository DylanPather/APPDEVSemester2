namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vehidinquotationtbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuotationTbls", "VehicleID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuotationTbls", "VehicleID");
        }
    }
}
