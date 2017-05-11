using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmDmSite.Models.SiteDataBase
{
    public class Performer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string PathToPhoto { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public int ViewsCount { get; set; }
        public int? SongsCount { get; set; }
        public Performer()
        {
            Songs = new List<Song>();
        }
    }
}