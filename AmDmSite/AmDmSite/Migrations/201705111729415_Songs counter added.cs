namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Songscounteradded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Performers", "SongsCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Performers", "SongsCount");
        }
    }
}
