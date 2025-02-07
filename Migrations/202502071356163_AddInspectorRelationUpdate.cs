namespace SoftMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInspectorRelationUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inspectors", "Id", "dbo.Inspections");
            DropIndex("dbo.Inspectors", new[] { "Id" });
            DropPrimaryKey("dbo.Inspectors");
            AlterColumn("dbo.Inspectors", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Inspectors", "Id");
            CreateIndex("dbo.Inspections", "InspectorId");
            AddForeignKey("dbo.Inspections", "InspectorId", "dbo.Inspectors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspections", "InspectorId", "dbo.Inspectors");
            DropIndex("dbo.Inspections", new[] { "InspectorId" });
            DropPrimaryKey("dbo.Inspectors");
            AlterColumn("dbo.Inspectors", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Inspectors", "Id");
            CreateIndex("dbo.Inspectors", "Id");
            AddForeignKey("dbo.Inspectors", "Id", "dbo.Inspections", "Id");
        }
    }
}
