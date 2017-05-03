namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PathToPicture = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Performers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Biography = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        Performer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performers", t => t.Performer_Id)
                .Index(t => t.Performer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "Performer_Id", "dbo.Performers");
            DropIndex("dbo.Songs", new[] { "Performer_Id" });
            DropTable("dbo.Songs");
            DropTable("dbo.Performers");
            DropTable("dbo.Accords");
        }
    }
}
