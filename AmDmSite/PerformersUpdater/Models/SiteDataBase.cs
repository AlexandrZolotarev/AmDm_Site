namespace PerformersUpdater.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SiteDataBase : DbContext
    {
        public SiteDataBase()
            : base("name=SiteDataBase")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Accord> Accords { get; set; }
        public virtual DbSet<Performer> Performers { get; set; }
        public virtual DbSet<Songs> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accord>()
                .HasMany(e => e.Songs)
                .WithMany(e => e.Accords)
                .Map(m => m.ToTable("SongAccords").MapLeftKey("Accord_Id").MapRightKey("Song_Id"));

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.Songs)
                .WithOptional(e => e.Performers)
                .HasForeignKey(e => e.PerformerId);
        }
    }
}
