namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApdatedataBasestructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SongAccords", "Song_Id", "dbo.Songs");
            DropForeignKey("dbo.SongAccords", "Accord_Id", "dbo.Accords");
            DropIndex("dbo.SongAccords", new[] { "Song_Id" });
            DropIndex("dbo.SongAccords", new[] { "Accord_Id" });
            RenameColumn(table: "dbo.Songs", name: "Performer_Id", newName: "PerformerId");
            RenameIndex(table: "dbo.Songs", name: "IX_Performer_Id", newName: "IX_PerformerId");
            AddColumn("dbo.Accords", "Song_Id", c => c.Int());
            CreateIndex("dbo.Accords", "Song_Id");
            AddForeignKey("dbo.Accords", "Song_Id", "dbo.Songs", "Id");
            DropTable("dbo.SongAccords");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SongAccords",
                c => new
                    {
                        Song_Id = c.Int(nullable: false),
                        Accord_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_Id, t.Accord_Id });
            
            DropForeignKey("dbo.Accords", "Song_Id", "dbo.Songs");
            DropIndex("dbo.Accords", new[] { "Song_Id" });
            DropColumn("dbo.Accords", "Song_Id");
            RenameIndex(table: "dbo.Songs", name: "IX_PerformerId", newName: "IX_Performer_Id");
            RenameColumn(table: "dbo.Songs", name: "PerformerId", newName: "Performer_Id");
            CreateIndex("dbo.SongAccords", "Accord_Id");
            CreateIndex("dbo.SongAccords", "Song_Id");
            AddForeignKey("dbo.SongAccords", "Accord_Id", "dbo.Accords", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SongAccords", "Song_Id", "dbo.Songs", "Id", cascadeDelete: true);
        }
    }
}
