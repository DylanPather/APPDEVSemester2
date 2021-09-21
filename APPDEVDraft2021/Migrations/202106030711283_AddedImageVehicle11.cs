namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageVehicle11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteCarts", "SellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteCarts", "SellingPrice");
        }
    }
}
