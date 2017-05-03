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
            // This code only for test
            List<Performer> p = HtmlAmDmParser.GetPerformersInfo(new List<Accord>(new SiteContext().Accords));
            SiteContext s = new SiteContext();
            s.Performers.Add(p[0]);
            s.SaveChanges();
            return View(new SiteContext().Performers);
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