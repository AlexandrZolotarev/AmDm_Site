using AmDmSite.HtmlParser;
using AmDmSite.Models.SiteDataBase;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmDmSite.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(int? page)

        {
            //Comment strings must save, while update function did't create

            //List<Performer> p = HtmlAmDmParser.GetPerformersInfo(new List<Accord>(new SiteContext().Accords));
            SiteContext s = new SiteContext();
            //foreach (Performer performer in p)
            //{
            //    s.Performers.Add(performer);
            //}
            //s.SaveChanges();
            List<Performer> performers = new List<Performer>(s.Performers);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(performers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Performer(int performerId, int? page)
        {
            using (SiteContext siteDataBase = new SiteContext())
            {
                Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
                ViewBag.PerformerName = performer.Name;
                ViewBag.PerformerBiography = performer.Biography;
                ViewBag.PerformerId = performerId;
                performer.ViewsCount++;
                siteDataBase.SaveChanges();
                int pageSize = 10;
                int pageNumber = (page ?? 1); 
                return View(performer.Songs.ToPagedList(pageNumber, pageSize));
            }      
        }

        public ActionResult Song(int songId)
        {
            SiteContext siteDataBase = new SiteContext();
            Song song = siteDataBase.Songs.FirstOrDefault(x => x.Id == songId);
            song.ViewsCount++;
            siteDataBase.SaveChanges();
            return View(song);
        }

    }
}