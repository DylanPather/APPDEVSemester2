namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostPriceSellPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockServiceTbls", "SellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StockServiceTbls", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockServiceTbls", "CostPrice");
            DropColumn("dbo.StockServiceTbls", "SellingPrice");
        }
    }
}
