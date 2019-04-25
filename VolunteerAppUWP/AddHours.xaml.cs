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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddHours : Page
    {
        User user;
        public AddHours(User user)
        {
            this.InitializeComponent();
            this.user = user;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainPage(user);
        }

        private async void BtnSub_Click(object sender, RoutedEventArgs e)
        {
            if (tbTit.Text != ""
                && tbHr.Text != ""
                && cdpDate.Date.HasValue)
            {
                DBConn.AddHours(user.UID, tbTit.Text, double.Parse(tbHr.Text), cdpDate.Date.Value.DateTime);
                Window.Current.Content = new MainPage(user);
            }
            else
            {
                tbTit.Text = string.Empty;
                tbHr.Text = string.Empty;
                MessageDialog md = new MessageDialog("Please Fill out all the Fields with valid input");
                await md.ShowAsync();
            }
        }
    }
}
