using HtmlAgilityPack;
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
    public static class PerformersHtmlParser
    {
        public static void Parse()
        {
            ParserContainer.ParsePerformersInfo();
        }
    }
}
