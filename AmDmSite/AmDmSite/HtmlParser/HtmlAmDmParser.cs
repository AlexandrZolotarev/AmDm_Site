using AmDmSite.Models.SiteDataBase;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace AmDmSite.HtmlParser
{
    public class HtmlAmDmParser
    {

        static string biography;
        public static List<Accord> accords = new List<Accord>();

        public static void GetPerformersInfo()
        {
            using (SiteContext s = new SiteContext()) {
               // s.Songs.RemoveRange(s.Songs);
               // s.Performers.RemoveRange(s.Performers);
               // s.SaveChanges();
                accords = new List<Accord>(s.Accords);
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
               for (int page = 0; page <= 10; page++)
                //for (int page = 1; page <= 1; page++)
                {
                    string str = web.DownloadString($"https://amdm.ru/chords/page" + page + "/");
                    str = HttpUtility.HtmlDecode(str);

                    HtmlDocument siteHtml = new HtmlDocument();
                    siteHtml.LoadHtml(str);
                    var rows = siteHtml.DocumentNode.SelectNodes(".//tr");

                    for (int i = 2; i <= 30; i++)
                    {
                        //for (int i = 1; i <= 20; i++)
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

        public static List<Song> GetPerformerSongsInfo(string linkToSongs)
        {
            List<Song> songs = new List<Song>();
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
                //for (int i = 1; i < rows.Count; i++)
                for (int i = 1; i < 3; i++)
                {
                    Thread.Sleep(800);
                    try
                    {
                        if (rows[i].SelectNodes(".//a") != null)
                        {
                            if (rows[i].SelectNodes(".//a")[0].Attributes[1].Value.Equals("g-link"))
                            {
                                Song song = new Song();
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

        public static Song GetSongInfo(string linkToInfo, Song song)
        {
            Thread.Sleep(900);
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            string str = web.DownloadString(linkToInfo);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var info = siteHtml.DocumentNode.SelectNodes(".//pre");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~Text~~~~~~~~~~~~~~~~~~~~~");
            song.Text = info[0].InnerText.Trim();
            var accordImages = siteHtml.GetElementbyId("song_chords").SelectNodes(".//img");
            if (accordImages != null)
                foreach (var accordImage in accordImages)
                {
                    if (!accords.Exists(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)))
                    {
                        Accord accord = new Accord() { PathToPicture = accordImage.Attributes[0].Value };
                        Regex rgx = new Regex(@"([^/]+)(?=_[^_]*$)");
                        accord.Name = rgx.Matches(accord.PathToPicture)[0].ToString();
                        accords.Add(accord);
                       
                        song.Accords.Add(accord);
                    }
                    else
                    {
                        song.Accords.Add(accords.Find(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)));
                    }
                }
            return song;
        }
    }
}