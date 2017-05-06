using AmDmSite.HtmlParser;
using AmDmSite.Models.SiteDataBase;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AmDmSite.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page, int? column, int? typeAscending)

        {
         // HtmlAmDmParser.GetPerformersInfo();
            SiteContext s = new SiteContext();
            List<Performer> performers = new List<Performer>(s.Performers);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            int colNumber = (column ?? 0);
            int ascendType = (typeAscending ?? -1);

            ViewBag.Page = pageNumber;
            ViewBag.NameType = 0;
            ViewBag.SongsType = 0;
            ViewBag.ViewsCountType = 0;

            if (ascendType == -1)
                return View(performers.ToPagedList(pageNumber, pageSize));

            switch (column)
            {
                case 1:
                    if (ascendType == 0)
                    {
                        ViewBag.NameType = 1;
                        return View(performers.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        ViewBag.NameType = 0;
                        return View(performers.OrderByDescending(x => x.Name).ToPagedList(pageNumber, pageSize));
                    }
                case 2:
                    if (ascendType == 0)
                    {
                        ViewBag.SongsType = 1;
                        return View(performers.OrderBy(x => x.Songs.Count).ToPagedList(pageNumber, pageSize));
                    }
                    else {
                        ViewBag.SongsType = 0;
                        return View(performers.OrderByDescending(x => x.Songs.Count).ToPagedList(pageNumber, pageSize));
            }
                case 3:

                    if (ascendType == 0)
                    {
                        ViewBag.ViewsCountType = 1;
                        return View(performers.OrderBy(x => x.ViewsCount).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        ViewBag.ViewsCountType = 0;
                        return View(performers.OrderByDescending(x => x.ViewsCount).ToPagedList(pageNumber, pageSize));
                    }
                default: return View(performers.ToPagedList(pageNumber, pageSize));
            }
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult About()
        {
            return View();
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Contact()
        {
            return View();
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Performer(int performerId, int? page)
        {
            using (SiteContext siteDataBase = new SiteContext())
            {
                Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
                ViewBag.PerformerName = performer.Name;
                ViewBag.PerformerBiography = performer.Biography;
                ViewBag.PerformerId = performerId;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                if (pageNumber == 1) performer.ViewsCount++;
                siteDataBase.SaveChanges();
                return View(performer.Songs.ToPagedList(pageNumber, pageSize));
            }
        }
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult SongInfo(int performerId, int songNumber)
        {
            SiteContext siteDataBase = new SiteContext();
            Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
            Song song = performer.Songs.FirstOrDefault(x => x.Number == songNumber);
            song.ViewsCount++;
            siteDataBase.SaveChanges();
            ViewBag.NextSong = performer.Songs.Count > song.Number + 1 ? song.Number + 1 : -1;
            ViewBag.PreviousSong = song.Number > 1 ? song.Number - 1 : -1;
            return PartialView(song);
        }

        

        public ActionResult ChangeSong(Song song)
        {
            SiteContext siteDataBase = new SiteContext();
            Song songToEdit = siteDataBase.Songs.FirstOrDefault(x => x.Id == song.Id);
            songToEdit.Text = song.Text;
            siteDataBase.SaveChanges();
            return RedirectToAction("Song", new { performerId = songToEdit.PerformerId, songNumber = songToEdit.Number });
        }

        public ActionResult ChangedSongInfo(int performerId, int songNumber)
        {
            SiteContext siteDataBase = new SiteContext();
            Performer performer = siteDataBase.Performers.FirstOrDefault(x => x.Id == performerId);
            Song song = performer.Songs.FirstOrDefault(x => x.Number == songNumber);
            return View(song);
        }

    }
}