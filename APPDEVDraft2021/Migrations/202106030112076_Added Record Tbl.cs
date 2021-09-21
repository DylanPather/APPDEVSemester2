namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRecordTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecordQuotes",
                c => new
                    {
                        RQuoteID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        VehicleID = c.String(),
                        CartID = c.String(),
                    })
                .PrimaryKey(t => t.RQuoteID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RecordQuotes");
        }
    }
}
