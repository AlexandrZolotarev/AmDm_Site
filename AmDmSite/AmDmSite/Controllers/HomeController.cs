using AmDmSite.HtmlParser;
using AmDmSite.Models.SiteDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmDmSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // This code only for test parsing && save time on testing

            //List<Performer> p = HtmlAmDmParser.GetPerformersInfo(new List<Accord>(new SiteContext().Accords));
            SiteContext s = new SiteContext();
            //s.Performers.Add(new Performer { Name = "Test", Biography = "sfs", ViewsCount = 124 });
            //s.SaveChanges();
            //foreach (Performer performer in p)
            //{
            //    s.Performers.Add(performer);
            //}
            //s.SaveChanges();
            List<Performer> performers = new List<Performer>(s.Performers);
            return View(performers);
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
    }
}