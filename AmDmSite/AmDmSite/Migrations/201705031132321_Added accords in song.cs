namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedaccordsinsong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accords", "Song_Id", c => c.Int());
            CreateIndex("dbo.Accords", "Song_Id");
            AddForeignKey("dbo.Accords", "Song_Id", "dbo.Songs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accords", "Song_Id", "dbo.Songs");
            DropIndex("dbo.Accords", new[] { "Song_Id" });
            DropColumn("dbo.Accords", "Song_Id");
        }
    }
}
