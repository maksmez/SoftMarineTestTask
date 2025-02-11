namespace SoftMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixInspection : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inspections", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inspections", "Comment", c => c.String(nullable: false));
        }
    }
}
