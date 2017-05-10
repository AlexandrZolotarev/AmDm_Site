using AmDmSite.Models.SiteDataBase;
using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace PerformersUpdater
{
    public class PerformersHtmlUpdater
    {
        public static List<Accord> accords = new List<Accord>();
        static Logger logger;
        static int songsCounter = 0;

        public static void Main()
        {
            logger = LogManager.GetCurrentClassLogger();
            UpdatePerformersInfo();
            try
            {
                new WebClient().DownloadString($"http://localhost:53067/Home/SendPushMessage?message=" + DateTime.Now + ",%20Updated " + songsCounter + " songs");
            }
            catch
            {
                logger.Error("Not found controller");
            }
            finally
            {
                logger.Info(DateTime.Now + " updated "+songsCounter+" songs");
            }
        }

        public static void UpdatePerformersInfo()
        {
            using (SiteDataBase s = new SiteDataBase())
            {
                accords = new List<Accord>(s.Accords);
                System.Net.WebClient web = new System.Net.WebClient();
                web.Encoding = UTF8Encoding.UTF8;
                for (int page = 1; page <= 10; page++)
                {
                    try
                    {
                        string str = web.DownloadString($"https://amdm.ru/chords/page" + page + "/");
                        str = HttpUtility.HtmlDecode(str);
                        HtmlDocument siteHtml = new HtmlDocument();
                        siteHtml.LoadHtml(str);
                        var rows = siteHtml.DocumentNode.SelectNodes(".//tr");
                        for (int i = 1; i <= 30; i++)
                        {
                            string name = rows[i].SelectNodes(".//a")[1].InnerText.Trim();
                            Performer performer = s.Performers.FirstOrDefault(x => x.Name.Equals(name));
                            if (performer != null)
                            {
                                UpdatePerformer(s, rows, i, name, performer);
                            }
                        }
                        Thread.Sleep(800);
                    }
                    catch (Exception exception) {
                        logger.Fatal("Page "+page+", " + exception.Message);
                    }
                 }
                }
            }

        private static void UpdatePerformer(SiteDataBase s, HtmlNodeCollection rows, int i, string name, Performer performer)
        {
            Console.WriteLine("Checking performer " + performer.Name);
            performer.Songs = GetPerformerSongsInfo("https:" + rows[i].SelectNodes(".//a")[1].Attributes[0].Value, performer);
            s.Performers.FirstOrDefault(x => x.Name.Equals(name)).Songs = performer.Songs;
            s.SaveChanges();
            Console.WriteLine("Complete checking performer " + performer.Name);
            Thread.Sleep(800);
        }

        public static List<Song> GetPerformerSongsInfo(string linkToSongs, Performer performer)
        {
            List<Song> songs = performer.Songs.ToList();
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            string str = web.DownloadString(linkToSongs);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var rows = siteHtml.DocumentNode.SelectNodes(".//tr");
            int updatedSongsCount = 0;
            if (rows != null)
            {
                for (int i = 1; i < rows.Count; i++)
                {
                    try
                    {
                        if (rows[i].SelectNodes(".//a") != null)
                        {
                            if (rows[i].SelectNodes(".//a")[0].Attributes[1].Value.Equals("g-link"))
                            {
                                if (songs.FirstOrDefault(x => x.Name.Equals(rows[i].SelectNodes(".//a")[0].InnerText.Trim())) == null)
                                {
                                    updatedSongsCount = UpdateSong(performer, songs, rows, updatedSongsCount, i);
                                }
                            }
                        }

                    }
                    catch (Exception exception)
                    {
                        logger.Fatal(exception.Message);
                    }
                }
                logger.Trace($"{performer.Name}: {updatedSongsCount} песен обновлены");


            }
            return songs;
        }

        private static int UpdateSong(Performer performer, List<Song> songs, HtmlNodeCollection rows, int updatedSongsCount, int i)
        {
            Song song = new Song();
            song.Name = rows[i].SelectNodes(".//a")[0].InnerText.Trim();
            song = GetSongInfo("https:" + rows[i].SelectNodes(".//a")[0].Attributes[0].Value, song);
            song.Number = i;
            songs.Add(song);
            updatedSongsCount++;
            Console.WriteLine("Added new song: '" + song.Name + "' to " + performer.Name);
            songsCounter++;
            Thread.Sleep(800);
            return updatedSongsCount;
        }

        public static Song GetSongInfo(string linkToInfo, Song song)
        {
            Thread.Sleep(750);
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            string str = web.DownloadString(linkToInfo);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var info = siteHtml.DocumentNode.SelectNodes(".//pre");
            song.Text = info[0].InnerText.Trim();
            var accordImages = siteHtml.GetElementbyId("song_chords").SelectNodes(".//img");
            if (accordImages != null)
                foreach (var accordImage in accordImages)
                {
                    if (!accords.Exists(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)))
                    {
                        UpdateAccord(song, accordImage);
                    }
                    else
                    {
                        song.Accords.Add(accords.Find(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)));
                    }
                }
            return song;
        }

        private static void UpdateAccord(Song song, HtmlNode accordImage)
        {
            Accord accord = new Accord() { PathToPicture = accordImage.Attributes[0].Value };
            Regex rgx = new Regex(@"([^/]+)(?=_[^_]*$)");
            accord.Name = rgx.Matches(accord.PathToPicture)[0].ToString();
            accords.Add(accord);
            song.Accords.Add(accord);
        }
    }
}