namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PerformerViewsadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Performers", "ViewsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Performers", "ViewsCount");
        }
    }
}
