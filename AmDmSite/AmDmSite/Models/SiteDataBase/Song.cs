using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmDmSite.Models.SiteDataBase
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Performer Performer { get; set; }
        public List<Accord> Accords { get; set; }
        public Song()
        {
            Accords = new List<Accord>();
            Performer = new Performer();
        }
    }
}