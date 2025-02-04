namespace SoftMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemark : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(),
                        InspectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .Index(t => t.InspectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Remarks", "InspectionId", "dbo.Inspections");
            DropIndex("dbo.Remarks", new[] { "InspectionId" });
            DropTable("dbo.Remarks");
        }
    }
}
