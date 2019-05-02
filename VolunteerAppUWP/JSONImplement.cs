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
using System.Net.NetworkInformation;

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

        public async static Task<Geopoint> getCurrentLocation()
        {
            //https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA
            Chilkat.Global glob = new Chilkat.Global();
            bool succ = glob.UnlockBundle("Anything for 30-day trial");
            if (succ != true)
            {
                Debug.WriteLine(glob.LastErrorText);
            }

            int status = glob.UnlockStatus;
            if (status == 2)
            {
                Debug.WriteLine("Unlocked using purchased unlock code.");
            }
            else
            {
                Debug.WriteLine("Unlocked in trial mode.");
            }

            Chilkat.Rest rest = new Chilkat.Rest();

            //  Connect to the Google API REST server.
            bool bTls = true;
            int port = 443;
            bool bAutoReconnect = true;
            bool success = await rest.ConnectAsync("www.googleapis.com", port, bTls, bAutoReconnect);

            //  Add the Content-Type request header.
            rest.AddHeader("Content-Type", "application/json");

            //  Add your API key as a query parameter
            rest.AddQueryParam("key", "AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA");

            //  The JSON query is contained in the body of the HTTP POST.
            //  This is a sample query (which we'll dynamically build in this example)
            //  {
            //   "homeMobileCountryCode": 310,
            //   "homeMobileNetworkCode": 260,
            //   "radioType": "gsm",
            //   "carrier": "T-Mobile",
            //   "cellTowers": [
            //    {
            //    "cellId": 39627456,
            //     "locationAreaCode": 40495,
            //     "mobileCountryCode": 310,
            //     "mobileNetworkCode": 260,
            //     "age": 0,
            //     "signalStrength": -95
            //    }
            //   ],
            //   "wifiAccessPoints": [
            //    {
            //     "macAddress": "01:23:45:67:89:AB",
            //     "signalStrength": 8,
            //     "age": 0,
            //     "signalToNoiseRatio": -65,
            //     "channel": 8
            //    },
            //    {
            //     "macAddress": "01:23:45:67:89:AC",
            //     "signalStrength": 4,
            //     "age": 0
            //    }
            //   ]
            //  }

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            /*json.AppendInt("homeMobileCountryCode", 310);
            json.AppendInt("homeMobileNetworkCode", 260);
            json.AppendString("radioType", "gsm");
            json.AppendString("carrier", "T-Mobile");
            Chilkat.JsonArray aCellTowers = json.AppendArray("cellTowers");
            aCellTowers.AddObjectAt(0);
            Chilkat.JsonObject oCellTower = aCellTowers.ObjectAt(0);
            oCellTower.AppendInt("cellId", 39627456);
            oCellTower.AppendInt("locationAreaCode", 40495);
            oCellTower.AppendInt("mobileCountryCode", 310);
            oCellTower.AppendInt("mobileNetworkCode", 260);
            oCellTower.AppendInt("age", 0);
            oCellTower.AppendInt("signalStrength", -95);*/

            var macAddr =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();

            Chilkat.JsonArray aWifi = json.AppendArray("wifiAccessPoints");
            aWifi.AddObjectAt(0);
            Chilkat.JsonObject oPoint = aWifi.ObjectAt(0);
            oPoint.AppendString("macAddress", macAddr);
            oPoint.AppendInt("signalStrength", 8);
            oPoint.AppendInt("age", 0);
            oPoint.AppendInt("signalToNoiseRatio", -65);
            oPoint.AppendInt("channel", 8);

            /*aWifi.AddObjectAt(1);
            oPoint = aWifi.ObjectAt(1);
            oPoint.AppendString("macAddress", macAddr);
            oPoint.AppendInt("signalStrength", 4);
            oPoint.AppendInt("age", 0);*/

            string responseJson = await rest.FullRequestStringAsync("POST", "/geolocation/v1/geolocate", json.Emit());
            if (rest.LastMethodSuccess != true)
            {
                Console.WriteLine(rest.LastErrorText);
            }

            //  When successful, the response code is 200.
            if (rest.ResponseStatusCode != 200)
            {
                //  Examine the request/response to see what happened.
                Console.WriteLine("response status code = " + Convert.ToString(rest.ResponseStatusCode));
                Console.WriteLine("response status text = " + rest.ResponseStatusText);
                Console.WriteLine("response header: " + rest.ResponseHeader);
                Console.WriteLine("response JSON: " + responseJson);
                Console.WriteLine("---");
                Console.WriteLine("LastRequestStartLine: " + rest.LastRequestStartLine);
                Console.WriteLine("LastRequestHeader: " + rest.LastRequestHeader);
            }

            json.EmitCompact = false;
            Console.WriteLine("JSON request body: " + json.Emit());

            //  The JSON response should look like this:
            //  {
            //   "location": {
            //   "lat": 37.4248297,
            //    "lng": -122.07346549999998
            //   },
            //   "accuracy": 1145.0
            //  }

            Console.WriteLine("JSON response: " + responseJson);

            Chilkat.JsonObject jsonResp = new Chilkat.JsonObject();
            jsonResp.Load(responseJson);

            Chilkat.JsonObject jsonLoc = jsonResp.ObjectOf("location");
            //  Any JSON value can be obtained as a string..
            string latitude = jsonLoc.StringOf("lat");
            Console.WriteLine("latitude = " + latitude);
            string longitude = jsonLoc.StringOf("lng");
            Console.WriteLine("longitude = " + longitude);
            BasicGeoposition bg = new BasicGeoposition();
            bg.Latitude = double.Parse(latitude);
            bg.Longitude = double.Parse(longitude);
            return new Geopoint(bg);
        }
    }
}
