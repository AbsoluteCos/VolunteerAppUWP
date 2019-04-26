using Syncfusion.UI.Xaml.CellGrid;
using Syncfusion.UI.Xaml.CellGrid.Styles;
using Syncfusion.UI.Xaml.Grid.Utility;
using Syncfusion.XlsIO.Implementation.PivotTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewHours : Page
    {
        User user;

        public ViewHours(User user)
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(rMenu);
            this.user = user;
            inittable();
        }

        private void inittable()
        {
            List<ServiceHours> shs = DBConn.GetServiceHours(user.UID);
            sfcg.RowCount = shs.Count + 1;
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
            sfcg.Model[0, 1].CellValue = "Hours";
            sfcg.Model[0, 1].BaseStyle = "custom";
            sfcg.Model[0, 2].CellValue = "Date";
            sfcg.Model[0, 2].BaseStyle = "custom";
            int row = 1;
            foreach (ServiceHours sh in shs)
            {
                sfcg.Model[row, 0].CellValue = sh.Title;
                sfcg.Model[row, 0].BaseStyle = "custom1";
                sfcg.Model[row, 1].CellValue = sh.Hours;
                sfcg.Model[row, 1].BaseStyle = "custom1";
                sfcg.Model[row, 2].CellValue = sh.Date.ToShortDateString();
                sfcg.Model[row, 2].BaseStyle = "custom1";
                row++;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainPage(user);
        }
    }
}
