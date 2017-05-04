using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmDmSite.Models.SiteDataBase
{
    public class Accord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToPicture { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public Accord()
        {
            Songs = new List<Song>();
        }
    }
}