namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateaccordstructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accords", "Song_Id", "dbo.Songs");
            DropIndex("dbo.Accords", new[] { "Song_Id" });
            CreateTable(
                "dbo.SongAccords",
                c => new
                    {
                        Song_Id = c.Int(nullable: false),
                        Accord_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_Id, t.Accord_Id })
                .ForeignKey("dbo.Songs", t => t.Song_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accords", t => t.Accord_Id, cascadeDelete: true)
                .Index(t => t.Song_Id)
                .Index(t => t.Accord_Id);
            
            DropColumn("dbo.Accords", "Song_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accords", "Song_Id", c => c.Int());
            DropForeignKey("dbo.SongAccords", "Accord_Id", "dbo.Accords");
            DropForeignKey("dbo.SongAccords", "Song_Id", "dbo.Songs");
            DropIndex("dbo.SongAccords", new[] { "Accord_Id" });
            DropIndex("dbo.SongAccords", new[] { "Song_Id" });
            DropTable("dbo.SongAccords");
            CreateIndex("dbo.Accords", "Song_Id");
            AddForeignKey("dbo.Accords", "Song_Id", "dbo.Songs", "Id");
        }
    }
}
