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
        static Logger logger;

        public static void Main()
        {
            Update();
        }

        public static void Update()
        {
            logger = LogManager.GetCurrentClassLogger();
            int songsCount = ParserContainer.UpdatePerformersInfo();
            Notify(songsCount);
        }

        private static void Notify(int songsCount)
        {
            try
            {
                new WebClient().DownloadString($"http://localhost:53067/Home/SendPushMessage?message=" + DateTime.Now + ",%20Updated " + songsCount + " songs");
            }
            catch
            {
                logger.Error("Not found controller");
            }
            finally
            {
                logger.Info(DateTime.Now + " updated " + songsCount + " songs");
            }
        }
    }
}