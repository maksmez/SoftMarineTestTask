namespace SoftMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInspectorRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inspectors",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Number = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Inspections", "InspectorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Inspections", "Comment", c => c.String(nullable: false));
            DropColumn("dbo.Inspections", "Inspector");
            DropColumn("dbo.Inspections", "Count");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inspections", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Inspections", "Inspector", c => c.String(nullable: false));
            DropForeignKey("dbo.Inspectors", "Id", "dbo.Inspections");
            DropIndex("dbo.Inspectors", new[] { "Id" });
            AlterColumn("dbo.Inspections", "Comment", c => c.String());
            DropColumn("dbo.Inspections", "InspectorId");
            DropTable("dbo.Inspectors");
        }
    }
}
