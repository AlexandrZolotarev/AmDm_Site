using AmDmSite.Models.SiteDataBase;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

namespace AmDmSite.HtmlParser
{
    public class HtmlAmDmParser
    {
        public static List<Accord> accords = new List<Accord>();

        public static List<Performer> GetPerformersInfo(List<Accord> accordsCollection)
        {
            accords = accordsCollection;
            List<Performer> performers = new List<Performer>();
           
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            for (int page = 1; page <= 1; page++)
            {
                string str = web.DownloadString($"https://amdm.ru/chords/page" + page + "/");
                str = HttpUtility.HtmlDecode(str);

                HtmlDocument siteHtml = new HtmlDocument();
                siteHtml.LoadHtml(str);
                var rows = siteHtml.DocumentNode.SelectNodes(".//tr");

                for (int i = 1; i <= 30; i++)
                {
                    Performer performer = new Performer();

                    var image = rows[i].SelectNodes(".//img");
                    performer.PathToPhoto = image[0].Attributes[0].Value;
                    performer.Name = rows[i].SelectNodes(".//a")[1].InnerText.Trim();
                    performer.Songs = GetPerformerSongsInfo("https:" + rows[i].SelectNodes(".//a")[1].Attributes[0].Value, performer);
                    performers.Add(performer);
                    Console.WriteLine();
                }
            }
            return performers;

        }

        public static List<Song> GetPerformerSongsInfo(string linkToSongs, Performer performer)
        {
            List<Song> songs = new List<Song>();
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;

            string str = web.DownloadString(linkToSongs);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var rows = siteHtml.DocumentNode.SelectNodes(".//tr");
            if (rows != null)
                for (int i = 1; i < rows.Count - 5; i++)
                {
                    Thread.Sleep(600);
                    if (rows[i].SelectNodes(".//a") != null)
                    {
                        Song song = new Song();
                        song.Name = rows[i].SelectNodes(".//a")[0].InnerText.Trim();
                        song.Performer = performer;
                       song = GetSongInfo("https:" + rows[i].SelectNodes(".//a")[0].Attributes[0].Value, song);
                        Console.WriteLine("_________________________________________");
                        songs.Add(song);
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
            song.Text=info[0].InnerText.Trim();
            var accordImages = siteHtml.GetElementbyId("song_chords").SelectNodes(".//img");
            if (accordImages != null)
                foreach (var accordImage in accordImages)
                {
                    if (!accords.Exists(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)))
                    {
                        Accord accord = new Accord() { PathToPicture = accordImage.Attributes[0].Value };
                        accords.Add(accord);
                        song.Accords.Add(accord);
                    } else
                    {
                        song.Accords.Add(accords.Find(x => x.PathToPicture.Equals(accordImage.Attributes[0].Value)));
                    }
                }
            return song;
        }
    }
}