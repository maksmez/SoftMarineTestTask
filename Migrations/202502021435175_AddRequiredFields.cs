namespace SoftMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inspections", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Inspections", "Inspector", c => c.String(nullable: false));
            AlterColumn("dbo.Remarks", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Remarks", "Text", c => c.String());
            AlterColumn("dbo.Inspections", "Inspector", c => c.String());
            AlterColumn("dbo.Inspections", "Name", c => c.String());
        }
    }
}
