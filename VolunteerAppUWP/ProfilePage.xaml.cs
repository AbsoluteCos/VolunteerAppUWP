﻿using System;
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
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage(double h, double w) // change height and width to size of frame
        {
            Height = h; Width = w;
            this.InitializeComponent();
        }

        private void AddHours_Click(object sender, RoutedEventArgs e)
        {
            //add pop-up winodow that has area to log hours, add photos, description, Title
            throw new NotImplementedException();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
