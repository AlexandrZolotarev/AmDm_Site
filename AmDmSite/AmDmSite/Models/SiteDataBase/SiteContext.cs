using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AmDmSite.Models.SiteDataBase
{
    public class SiteContext : DbContext
    {
        public SiteContext()
            :base("DbConnection")
        { }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Accord> Accords { get; set; }
    }
}