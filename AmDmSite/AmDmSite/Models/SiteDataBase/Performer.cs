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
        public List<Song> Songs { get; set; } 
    }
}