using Newtonsoft.Json;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Net;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Syndication;

namespace VolunteerAppUWP
{
    public static class JSONImplement
    {
        public static async Task<VOpp> getOppurtunities(CancellationToken ct)
        {
            using (HttpClient hc = new HttpClient())
            using (HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Get, "https://api.betterplace.org/de/api_v4/projects.json?facets=completed%3Afalse"))
            using (var response = await hc.SendAsync(hrm, ct))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VOpp>(content);
            }
        }

        public static List<string> getRssLinks()
        {
            List<string> links = new List<string>();
            string url = "https://www.volunteer.gov/vg.xml";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                string link = item.Links.First().Uri.AbsoluteUri;
                links.Add(link);
            }
            return links;
        }

        public static async Task<Data> getRSSOppurtunity(string link)
        {
            HtmlWeb web = new HtmlWeb();
			
			try // change this to let the method return a Data and community calls it then plots it then calls it then plots it so on...
			{
				HtmlDocument doc = await web.LoadFromWebAsync(link);
				var addressList = doc.DocumentNode.SelectNodes("//span[@class='black1']").ToList(); // address should be first
				var titleList = doc.DocumentNode.SelectNodes("//a[@href='javascript:void(0)']").ToList(); // title = title.First().InnerText;
				var descList = doc.DocumentNode.SelectNodes("//p").ToList();
				string addressText = addressList.First().InnerText;
				string titleText = titleList.First().InnerText;
				string descText = string.Empty;
				foreach (HtmlNode hn in descList)
				{
					if (hn.InnerText != string.Empty)
					{
						if (hn.InnerText == "&nbsp;") descText += "\n";
						else descText += hn.InnerText;
					}
				}
				Geopoint addr = getLocationFromAddress(addressText);
				return new Data { title = titleText,
												summary = descText,
												latitude = addr.Position.Latitude,
												longitude = addr.Position.Longitude };
			} catch (Exception e) { Debug.WriteLine(e.Message); return null; }
        }

        private static Geopoint getLocationFromAddress(string address)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false&key=AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA", Uri.EscapeDataString(address));

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            XElement lat = locationElement.Element("lat");
            XElement lng = locationElement.Element("lng");

            BasicGeoposition bg = new BasicGeoposition();
            bg.Latitude = double.Parse(lat.Value);
            bg.Longitude = double.Parse(lng.Value);

            return new Geopoint(bg);
        }
    }
}
