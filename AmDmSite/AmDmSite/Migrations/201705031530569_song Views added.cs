namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class songViewsadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "ViewsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "ViewsCount");
        }
    }
}
