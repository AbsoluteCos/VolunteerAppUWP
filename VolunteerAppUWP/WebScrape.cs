using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace VolunteerAppUWP
{
    public static class WebScrape
    {
        public static void GetVolunteerOppurtunities()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("");
            //geo locate and then go to volunteer websites and search under state or just do all states and have them all load on the map, might want to change this to an asyncronous function and while awaiting, display loading circle.
        }
    }
}
