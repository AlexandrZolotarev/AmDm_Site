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

            //HtmlAmDmParser.GetPerformersInfo();
            SiteContext s = new SiteContext();
            List<Performer> performers = new List<Performer>(s.Performers);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(performers.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize));
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

        public ActionResult Song(int performerId, int songNumber)
        {
            SiteContext siteDataBase = new SiteContext();
            Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
            Song song = performer.Songs.FirstOrDefault(x => x.Number == songNumber);
            song.ViewsCount++;
            siteDataBase.SaveChanges();
            ViewBag.NextSong = performer.Songs.Count > song.Number + 1 ? song.Number + 1 : -1;
            ViewBag.PreviousSong = song.Number > 1 ? song.Number - 1 : -1;
            return View(song);
        }

        public ActionResult SongInfo(int performerId, int songNumber)
        {
            bool check = Request.IsAjaxRequest();
            SiteContext siteDataBase = new SiteContext();
            Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
            Song song = performer.Songs.FirstOrDefault(x => x.Number == songNumber);
            song.ViewsCount++;
            siteDataBase.SaveChanges();
            ViewBag.NextSong = performer.Songs.Count > song.Number + 1 ? song.Number + 1 : -1;
            ViewBag.PreviousSong = song.Number > 1 ? song.Number - 1 : -1;
            return PartialView(song);
        }
    }
}