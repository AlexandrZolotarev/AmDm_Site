using HtmlAgilityPack;
using System;
using System.Text;
using System.Threading;
using System.Web;

namespace AmDmSite.HtmlParser
{
    public class HtmlAmDmParser
    {
        static void Main(string[] args)
        {
            GetPerformersInfo();
            Console.ReadLine();
        }

        public static void GetPerformersInfo()
        {

            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            for (int page = 1; page <= 10; page++)
            {
                string str = web.DownloadString($"https://amdm.ru/chords/page" + page + "/");
                str = HttpUtility.HtmlDecode(str);

                HtmlDocument siteHtml = new HtmlDocument();
                siteHtml.LoadHtml(str);
                var rows = siteHtml.DocumentNode.SelectNodes(".//tr");

                for (int i = 1; i <= 30; i++)
                {
                    Console.WriteLine("Исполнитель: ");
                    var image = rows[i].SelectNodes(".//img");
                    Console.Write(image[0].Attributes[0].Value + " ==> ");
                    Console.Write(rows[i].SelectNodes(".//a")[1].InnerText.Trim());
                    Console.Write(rows[i].SelectNodes(".//a")[1].Attributes[0].Value);
                    GetPerformerSongsInfo("https:" + rows[i].SelectNodes(".//a")[1].Attributes[0].Value);
                    Console.WriteLine();
                }
            }

        }

        public static void GetPerformerSongsInfo(string linkToSongs)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;
            Thread.Sleep(500);
            string str = web.DownloadString(linkToSongs);
            str = HttpUtility.HtmlDecode(str);
            HtmlDocument siteHtml = new HtmlDocument();
            siteHtml.LoadHtml(str);
            var rows = siteHtml.DocumentNode.SelectNodes(".//tr");

            for (int i = 1; i < rows.Count - 5; i++)
            {
                if (rows[i].SelectNodes(".//a") != null)
                {
                    Console.WriteLine("          Song:" + rows[i].SelectNodes(".//a")[0].InnerText.Trim());
                    Console.WriteLine("          Link to Text :" + rows[i].SelectNodes(".//a")[0].Attributes[0].Value);
                    Console.WriteLine("_________________________________________");

                }
            }
        }

        public static void GetSongInfo(string linkToInfo)
        {

        }
    }
}