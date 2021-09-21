namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceTbls",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        QuoteID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateOfInvoice = c.DateTime(nullable: false),
                        PaymentType = c.String(),
                    })
                .PrimaryKey(t => t.InvoiceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InvoiceTbls");
        }
    }
}
