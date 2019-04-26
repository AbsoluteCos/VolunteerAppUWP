using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Syncfusion.Licensing;
using Syncfusion.UI.Xaml.Reports;
using System.Reflection;
using Syncfusion.UI.Xaml.Charts;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        User user;
        public ProfilePage(User user) // change height and width to size of frame
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTQzMTJAMzEzNzJlMzEyZTMwQlE3TzZmOG1jR1J1TlRTUnIyQlVoczFqZmFQd0ZoZnUzSGpXVzVEZFo4UT0=");
            this.InitializeComponent();
            this.user = user;
            initgraph();
            initstats();
            initphoto();
        }

        private void initphoto()
        {
            string photoLink = DBConn.GetPhoto(user.UID);
            if (photoLink == null)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Click to Upload Photo",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Photo.Children.Add(tb);
                return;
            }
            Photo.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(this.BaseUri, Directory.GetCurrentDirectory() + $@"\Assets\{photoLink}")), Stretch = Stretch.UniformToFill };
        }

        private void initstats()
        {
            tbHours.Text += DBConn.GetTotalHours(user.UID);
            tbEvents.Text += DBConn.GetTotalEvents(user.UID);
            tbYears.Text += DBConn.GetTotalYears(user.UID);
        }

        private void initgraph()
        {
            List<ServiceHours> sh = DBConn.GetServiceHours(user.UID);
            List<ServiceHourImplementation> toImplement = new List<ServiceHourImplementation>();
            toImplement.Add(new ServiceHourImplementation { month=Month.January, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.February, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.March, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.April, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.May, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.June, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.July, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.August, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.September, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.October, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.November, hours=0 });
            toImplement.Add(new ServiceHourImplementation { month=Month.December, hours=0 });
            foreach (ServiceHours shs in sh)
            {
                if (shs.Date.Year == DateTime.Now.Year)
                {
                    foreach (ServiceHourImplementation shi in toImplement)
                    {
                        Debug.WriteLine((Month)(shs.Date.Month - 1));
                        if (shi.month == (Month)(shs.Date.Month - 1))
                        {
                            shi.hours += shs.Hours;
                            break;
                        }
                    }
                }
            }
            SfAreaSparkline sparkline = new SfAreaSparkline()
            {
                ItemsSource = toImplement,
                YBindingPath = "hours",
                XBindingPath = "month",
                MarkerVisibility = Visibility.Visible,
                ShowAxis = true,
                ShowTrackBall = true,
                Background = new SolidColorBrush(new Color { R=7, G=79, B=87, A=255 }),
                Margin = new Thickness(0,0,0,50)
            };
            sparkline.OnSparklineMouseMove += Sparkline_OnSparklineMouseMove;
            template.Children.Add(sparkline);
            Grid.SetColumn(sparkline, 1);
        }

        ContentPresenter info;
        private void Sparkline_OnSparklineMouseMove(object sender, SparklineMouseMoveEventArgs args)
        {
            if (!args.RootPanel.Children.Contains(info))

            {

                info = new ContentPresenter();

                args.RootPanel.Children.Add(info);

                info.Foreground = new SolidColorBrush(Colors.Red);

                info.FontSize = 25;

            }

            info.Content = args.Value.Y;

            info.Arrange(new Rect(args.Coordinate.X, args.Coordinate.Y, info.ActualWidth, info.ActualHeight));
        }

        private void AddHours_Click(object sender, RoutedEventArgs e)
        {
            //area to log hours, add photos, description, Title
            //thing to change frmain to addhours

            Window.Current.Content = new AddHours(user);
        }

        private void ViewHours_Click(object sender, RoutedEventArgs e)
        {
            //view all logged hours
            Window.Current.Content = new ViewHours(user);
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new LogIn();
        }

        private async void Photo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker fop = new FileOpenPicker();
            fop.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            StorageFile x = await fop.PickSingleFileAsync();
            Debug.WriteLine(Directory.GetCurrentDirectory());
            StorageFolder sf = await StorageFolder.GetFolderFromPathAsync(Directory.GetCurrentDirectory() + @"\Assets");
            try { await x.CopyAsync(sf); } catch { }
            DBConn.SetPhoto(user.UID, x.Name);
            initphoto();
        }
    }

    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public class ServiceHourImplementation
    {
        public Month month { get; set; }
        public double hours { get; set; }
    }
}
