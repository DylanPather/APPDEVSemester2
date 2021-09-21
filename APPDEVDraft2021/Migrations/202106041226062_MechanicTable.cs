namespace APPDEVDraft2021.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MechanicTbls",
                c => new
                    {
                        MechanicID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ContactNo = c.String(),
                    })
                .PrimaryKey(t => t.MechanicID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MechanicTbls");
        }
    }
}
