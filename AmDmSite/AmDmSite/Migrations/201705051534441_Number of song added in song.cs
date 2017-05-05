namespace AmDmSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Numberofsongaddedinsong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Number");
        }
    }
}
