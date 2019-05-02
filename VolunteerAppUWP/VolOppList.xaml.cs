using Syncfusion.UI.Xaml.CellGrid.Styles;
using Syncfusion.UI.Xaml.Grid.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VolOppList : Page
    {
        public VolOppList()
        {
            this.InitializeComponent();
            inittable();
        }

        private async void inittable(double radius = 0)
        {
            List<string> links = JSONImplement.getRssLinks();
            links.RemoveRange(50, links.Count - 50);
            Debug.WriteLine(links.Count);
            List<Data> opp = new List<Data>();
            // add something here that puts a thing that says loading data...
            foreach (string link in links)
                opp.Add(await JSONImplement.getRSSOppurtunity(link));
            if (radius != 0)
            {
                Geopoint xy = await JSONImplement.getCurrentLocation();
                List<int> toRemove = new List<int>();
                int i = 0;
                const double lengthAtEquator = 69.172;
                double radLat = Math.Cos(xy.Position.Latitude * Math.PI / 180) * lengthAtEquator;
                foreach (Data d in opp)
                {
                    double distance = Math.Sqrt(Math.Pow(d.latitude - xy.Position.Latitude, 2) * Math.Pow(d.longitude - xy.Position.Longitude, 2));
                    Debug.WriteLine(distance);
                    if (distance > radLat)
                    {
                        toRemove.Add(i - toRemove.Count); // to adjust for more things dying
                    }
                    i++;
                }
                foreach (int f in toRemove)
                    opp.RemoveAt(f);
            }
            Debug.WriteLine("got through loops");
            sfcg.RowCount = opp.Count + 1;
            GridStyleInfo baseStyle = sfcg.Model.BaseStylesMap["custom"].StyleInfo;
            baseStyle.Background = new SolidColorBrush(Colors.Gray);
            baseStyle.HorizontalAlignment = HorizontalAlignment.Center;
            baseStyle.Font.FontStyle = Windows.UI.Text.FontStyle.Oblique;
            GridStyleInfo baseStyle2 = sfcg.Model.BaseStylesMap["custom1"].StyleInfo;
            baseStyle2.Background = new SolidColorBrush(Colors.White);
            baseStyle2.HorizontalAlignment = HorizontalAlignment.Center;
            baseStyle2.VerticalAlignment = VerticalAlignment.Center;
            sfcg.Model.TableStyle.Borders.All = new Pen(new SolidColorBrush(Colors.Black), 2, BorderStyle.Standard);

            sfcg.Model[0, 0].CellValue = "Title";
            sfcg.Model[0, 0].BaseStyle = "custom";
            sfcg.Model[0, 1].CellValue = "Description";
            sfcg.Model[0, 1].BaseStyle = "custom";
            sfcg.Model[0, 2].CellValue = "Address";
            sfcg.Model[0, 2].BaseStyle = "custom";
            int row = 1;
            foreach (Data d in opp)
            {
                sfcg.Model[row, 0].CellValue = d.title;
                sfcg.Model[row, 0].BaseStyle = "custom1";
                sfcg.Model[row, 1].CellValue = d.summary;
                sfcg.Model[row, 1].BaseStyle = "custom1";
                sfcg.Model[row, 2].CellValue = d.latitude;
                sfcg.Model[row, 2].BaseStyle = "custom1";
                row++;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbMiRad.Text != string.Empty)
            {
                double rad = 0;
                if (double.TryParse(tbMiRad.Text, out rad))
                {
                    inittable(rad);
                }
                else
                {
                    MessageDialog message = new MessageDialog("Please enter a valid integer");
                    tbMiRad.Text = string.Empty;
                }
            }
        }
    }
}
