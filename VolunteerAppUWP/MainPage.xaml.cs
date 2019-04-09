﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Window.Current.SetTitleBar(rtitle);
            frmain.Content = new Welcome();
            lbMenu.SelectionChanged += MenuSelect;
        }

        private void MenuSelect(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = (ListBoxItem)lbMenu.Items.ElementAt(lbMenu.SelectedIndex);

            switch (lbi.Name)
            {
                case "lbiW":
                    frmain.Content = new Welcome();
                    break;
                case "lbiM":
                    frmain.Content = new MapDisplay();
                    break;
                case "lbiP":
                    frmain.Content = new ProfilePage(Height, Width);
                    break;
                case "lbiC":
                    frmain.Content = new CommunityPage();
                    break;
            }

            toggleMenu();
        }

        private void BtnHam_Click(object sender, RoutedEventArgs e)
        {
            toggleMenu();
        }

        private void toggleMenu()
        {
            Thickness open = new Thickness(0, 0, 0, 0);
            if (rslide.Margin != open)
                rslide.Margin = open;
            else
                rslide.Margin = new Thickness(-300, 0, 0, 0);

            if (Canvas.GetZIndex(rblur) == -1)
                Canvas.SetZIndex(rblur, 1);
            else
                Canvas.SetZIndex(rblur, -1);
        }

        private void Rblur_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            toggleMenu();
        }
    }
}