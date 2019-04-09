using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MapDisplay()
        {
            GMapsUWP.Initializer.Initialize("AIzaSyBxO61LSkP3OSzjXIG6J5vJC9ziC7tFYzA", "en-US");
            GMapsUWP.Map.MapControlHelper.UseGoogleMaps(mMain);
            InitializeComponent();

            List<MapElement> MyLandmarks = new List<MapElement>();
            Geopoint gp = new Geopoint(new BasicGeoposition { Latitude = 47.620, Longitude = -122.349 });
            var test = new MapIcon
            {
                Location = gp,
                NormalizedAnchorPoint = new Point(.5, 1),
                ZIndex = 0,
                Title = "Space Needle"
            };
            MyLandmarks.Add(test);
            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyLandmarks
            };
            mMain.Layers.Add(LandmarksLayer);
            mMain.Center = gp;
            mMain.ZoomLevel = 14;
        }
    }
}
