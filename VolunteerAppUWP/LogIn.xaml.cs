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
    public sealed partial class LogIn : Page
    {
        public LogIn()
        {
            this.InitializeComponent();
        }

        private async void BtnLI_Click(object sender, RoutedEventArgs e)
        {
            User user = DBConn.GetUser(tbUN.Text, tbPass.Password);
            if (user != null)
                Window.Current.Content = new MainPage(user);
            else
            {
                tbUN.Text = string.Empty;
                tbPass.Password = string.Empty;
                MessageDialog md = new MessageDialog("Invalid Username or Password");
                await md.ShowAsync();
            }
        }
    }
}
