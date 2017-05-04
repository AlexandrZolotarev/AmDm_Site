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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Performer(int performerId, int? page)
        {
                Performer performer = new SiteContext().Performers.FirstOrDefault(x => x.Id == performerId);
            ViewBag.PerformerName = performer.Name;
            ViewBag.PerformerBiography = performer.Biography;
            ViewBag.PerformerId = performerId;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(performer.Songs.ToPagedList(pageNumber, pageSize));       
        }

        public ActionResult Song(int songId)
        {
            return View(new SiteContext().Songs.FirstOrDefault(x => x.Id == songId));
        }

    }
}