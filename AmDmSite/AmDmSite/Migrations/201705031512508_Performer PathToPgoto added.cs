namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PerformerPathToPgotoadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Performers", "PathToPhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Performers", "PathToPhoto");
        }
    }
}
