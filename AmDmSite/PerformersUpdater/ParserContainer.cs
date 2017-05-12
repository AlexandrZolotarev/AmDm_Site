using HtmlAgilityPack;
using NLog;
using PerformersUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PerformersUpdater
{
    public static class ParserContainer
    {
        static string biography;
        public static List<Accord> accords = new List<Accord>();
        static Logger logger = LogManager.GetCurrentClassLogger();
        static int updatedSongsCount = 0;

        public static int UpdatePerformersInfo()
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
                    catch 
                    {

                    }
                }
            }
            return updatedSongsCount;
        }

        public static void ParsePerformersInfo()
        {
            using (SiteDataBase s = new SiteDataBase())
            {
                accords = new List<Accord>(s.Accords);
                System.Net.WebClient web = new System.Net.WebClient();
                web.Encoding = UTF8Encoding.UTF8;
                for (int page = 0; page <= 10; page++)
                {
                    string str = web.DownloadString($"https://amdm.ru/chords/page" + page + "/");
                    str = HttpUtility.HtmlDecode(str);

                    HtmlDocument siteHtml = new HtmlDocument();
                    siteHtml.LoadHtml(str);
                    var rows = siteHtml.DocumentNode.SelectNodes(".//tr");
                    for (int i = 1; i <= 30; i++)
                    {
                        try
                        {
                            Performer performer = new Performer();
                            var image = rows[i].SelectNodes(".//img");
                            performer.PathToPhoto = image[0].Attributes[0].Value;
                            performer.Name = rows[i].SelectNodes(".//a")[1].InnerText.Trim();
                            if (s.Performers.FirstOrDefault(x => x.Name.Equals(performer.Name)) == null)
                            {
                                performer.Songs = GetPerformerSongsInfo("https:" + rows[i].SelectNodes(".//a")[1].Attributes[0].Value);
                                performer.Biography = biography;
                                s.Performers.Add(performer);
                                s.SaveChanges();
                                Console.WriteLine("Added performer " + performer.Name);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public static List<Songs> GetPerformerSongsInfo(string linkToSongs)
        {
            List<Songs> songs = new List<Songs>();
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;

            string str = web.DownloadString(linkToSongs);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var biographyQuery = siteHtml.DocumentNode
    .Descendants("div")
    .Where(d =>
       d.Attributes.Contains("class")
       &&
       d.Attributes["class"].Value.Contains("artist-profile__bio")
    );
            biography = biographyQuery.ToList()[0].InnerText;
            var rows = siteHtml.DocumentNode.SelectNodes(".//tr");

            if (rows != null)
            {
                for (int i = 1; i < rows.Count; i++)
                {
                    Thread.Sleep(800);
                    try
                    {
                        if (rows[i].SelectNodes(".//a") != null)
                        {
                            if (rows[i].SelectNodes(".//a")[0].Attributes[1].Value.Equals("g-link"))
                            {
                                Songs song = new Songs();
                                song.Name = rows[i].SelectNodes(".//a")[0].InnerText.Trim();
                                song = GetSongInfo("https:" + rows[i].SelectNodes(".//a")[0].Attributes[0].Value, song);
                                Console.WriteLine("_________________________________________");
                                song.Number = i;
                                songs.Add(song);
                                Console.WriteLine("Added song " + song.Name);
                            }
                        }
                    }
                    catch { }
                }
            }
            return songs;
        }

        private static void UpdatePerformer(SiteDataBase s, HtmlNodeCollection rows, int i, string name, Performer performer)
        {
            Console.WriteLine("Checking performer " + performer.Name);
            performer.Songs = GetPerformerSongsInfo("https:" + rows[i].SelectNodes(".//a")[1].Attributes[0].Value, performer);
            s.Performers.FirstOrDefault(x => x.Name.Equals(name)).Songs = performer.Songs;
            s.Performers.FirstOrDefault(x => x.Name.Equals(name)).SongsCount = performer.Songs.Count;
            s.SaveChanges();
            Console.WriteLine("Complete checking performer " + performer.Name);
            Thread.Sleep(800);
        }

        public static List<Songs> GetPerformerSongsInfo(string linkToSongs, Performer performer)
        {
            List<Songs> songs = performer.Songs.ToList();
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            string str = web.DownloadString(linkToSongs);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var rows = siteHtml.DocumentNode.SelectNodes(".//tr");
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
                                    updatedSongsCount += UpdateSong(performer, songs, rows, updatedSongsCount, i);
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

        private static int UpdateSong(Performer performer, List<Songs> songs, HtmlNodeCollection rows, int updatedSongsCount, int i)
        {
            int songsCounter=0;
            Songs song = new Songs();
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

        public static Songs GetSongInfo(string linkToInfo, Songs songs)
        {
            Thread.Sleep(750);
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            string str = web.DownloadString(linkToInfo);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var info = siteHtml.DocumentNode.SelectNodes(".//pre");
            songs.Text = info[0].InnerText.Trim();
            var accordImages = siteHtml.GetElementbyId("song_chords").SelectNodes(".//img");
            if (accordImages != null)
                foreach (var accordImage in accordImages)
                {
                    if (!accords.Exists(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)))
                    {
                        UpdateAccord(songs, accordImage);
                    }
                    else
                    {
                        songs.Accords.Add(accords.Find(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)));
                    }
                }
            return songs;
        }

        private static void UpdateAccord(Songs song, HtmlNode accordImage)
        {
            Accord accord = new Accord() { PathToPicture = accordImage.Attributes[0].Value };
            Regex rgx = new Regex(@"([^/]+)(?=_[^_]*$)");
            accord.Name = rgx.Matches(accord.PathToPicture)[0].ToString();
            accords.Add(accord);
            song.Accords.Add(accord);
        }
    }
}
