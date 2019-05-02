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

        private void inittable()
        {
            

            addvalues();
        }

        private async void addvalues(double radius = 0)
        {
            List<string> links = JSONImplement.getRssLinks();
            links.RemoveRange(50, links.Count - 50);
            Debug.WriteLine(links.Count);
            List<Data> opp = new List<Data>();
            // add something here that puts a thing that says loading data...
            foreach (string link in links)
                opp.Add(await JSONImplement.getRSSOppurtunity(link));
            Geopoint xy = await JSONImplement.getCurrentLocation();
            if (radius != 0)
            {
                List<int> toRemove = new List<int>();
                int i = 0;
                int removed = 0;
                foreach (Data d in opp)
                {
                    double distance = JSONImplement.GetDistance(xy.Position.Latitude, xy.Position.Longitude, d.latitude, d.longitude);
                    Debug.WriteLine(distance);
                    if (distance > radius)
                    {
                        Debug.WriteLine("REMOVING --------> " + distance + " at index - count ======= " + (i));
                        toRemove.Add(i - removed++); // to adjust for more things dying
                    }
                    i++;
                }
                foreach (int f in toRemove)
                {
                    opp.RemoveAt(f);
                    Debug.WriteLine("REMOVING ACTUALLY ======= " + f);
                }
            }
            Debug.WriteLine("got through loops");

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
            sfcg.Model[0, 3].CellValue = "Distance to Current Location";
            sfcg.Model[0, 3].BaseStyle = "custom";

            if (sfcg.RowCount != 1) sfcg.Model.RemoveRows(1, sfcg.RowCount - 1);

            sfcg.RowCount = opp.Count + 1;


            int row = 1;
            foreach (Data d in opp)
            {
                sfcg.Model[row, 0].CellValue = d.title;
                sfcg.Model[row, 0].BaseStyle = "custom1";
                sfcg.Model[row, 1].CellValue = d.summary;
                sfcg.Model[row, 1].BaseStyle = "custom1";
                sfcg.Model[row, 2].CellValue = JSONImplement.GetAddress(d.latitude, d.longitude);
                sfcg.Model[row, 2].BaseStyle = "custom1";
                sfcg.Model[row, 3].CellValue = JSONImplement.GetDistance(xy.Position.Latitude, xy.Position.Longitude, d.latitude, d.longitude) + " mi";
                sfcg.Model[row, 3].BaseStyle = "custom1";
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
                    addvalues(rad);
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
