using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
// API Key: AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA
// Website: https://www.codeproject.com/Tips/1233432/Using-Google-Maps-API-in-UWP-Csharp

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapDisplay : Page
    {
        VOpp points;
        List<Data> morePoints;

        public MapDisplay()
        {
            GMapsUWP.Initializer.Initialize("AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA", "en-US");
            GMapsUWP.Map.MapControlHelper.UseGoogleMaps(mMain);
            InitializeComponent();

            DisplayMapData();
            DisplayRSSData();
        }

        private async void DisplayRSSData()
        {
            List<MapElement> RSSVolunteerLocations = new List<MapElement>();
            morePoints = new List<Data>();
            List<string> links = JSONImplement.getRssLinks();
            links.RemoveRange(100, links.Count - 100); // maybe make master method that seperate links into different lists and different async methods
            foreach (string link in links)
            {
                Data x = await JSONImplement.getRSSOppurtunity(link);
                morePoints.Add(x);
                Geopoint gp = new Geopoint(new BasicGeoposition { Latitude = x.latitude, Longitude = x.longitude });
                var xy = new MapIcon
                {
                    Location = gp,
                    NormalizedAnchorPoint = new Point(.5, 1),
                    ZIndex = 0,
                    Title = x.title
                };
                RSSVolunteerLocations.Add(xy);
            }
            var LocLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = RSSVolunteerLocations
            };
            mMain.Layers.Add(LocLayer);
            Debug.WriteLine("finish big rss boi");
        }

        private async void DisplayMapData()
        {
            CancellationTokenSource cts = new CancellationTokenSource(); // can use this to stop the method after a time
            points = await JSONImplement.getOppurtunities(cts.Token);
            //morePoints = await JSONImplement.getRSSOppurtunities();

            Debug.WriteLine(points.data.Count);

            List<MapElement> VolunteerLocations = new List<MapElement>();

            foreach (Data d in points.data)
            {
                Geopoint gp = new Geopoint(new BasicGeoposition { Latitude = d.latitude, Longitude = d.longitude });
                var xy = new MapIcon
                {
                    Location = gp,
                    NormalizedAnchorPoint = new Point(.5, 1),
                    ZIndex = 0,
                    Title = d.title
                };
                VolunteerLocations.Add(xy);
            }

            /*foreach (Data d in morePoints)
            {
                Geopoint gp = new Geopoint(new BasicGeoposition { Latitude = d.latitude, Longitude = d.longitude });
                var xy = new MapIcon
                {
                    Location = gp,
                    NormalizedAnchorPoint = new Point(.5, 1),
                    ZIndex = 0,
                    Title = d.title
                };
                VolunteerLocations.Add(xy);
            }*/

            var LocLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = VolunteerLocations
            };
            mMain.Layers.Add(LocLayer);
            
        }

        private void Landmark_Click(MapControl sender, MapElementClickEventArgs mecea)
        {
            var elements = mecea.MapElements;
            MapIcon element = mecea.MapElements.First<MapElement>() as MapIcon;
            Debug.WriteLine(element.Title);
            Data pnt = new Data();
            foreach (Data d in points.data)
                if (d.title == element.Title)
                    pnt = d;
            foreach (Data d in morePoints)
                if (d.title == element.Title)
                    pnt = d;
            if (pnt == null)
                return;
            DisplayData(pnt.title, pnt.summary);
        }

        private async void DisplayData(string title, string description)
        {
            ContentDialog displayPoint = new ContentDialog
            {
                Title = title,
                Content = description,
                CloseButtonText = "Ok"
            };
            await displayPoint.ShowAsync();
        }

        private string TranslateText()
        {
            return String.Empty;
        }
    }
}
