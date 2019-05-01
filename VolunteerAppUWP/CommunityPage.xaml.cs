using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VolunteerAppUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommunityPage : Page
    {
        User user;

        public CommunityPage(User user)
        {
            this.InitializeComponent();
            this.user = user;
            ShowPosts(RetrievePosts(0));
        }

        private void LVI_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)sender;
            lvi.Background = new SolidColorBrush(Color.FromArgb(255,139,143,165));
        }

        private void LVI_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)sender;
            lvi.Background = new SolidColorBrush(Color.FromArgb(255, 74, 75, 81));
        }

        private void lvTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spFeed.Children.Clear();
            ShowPosts(RetrievePosts(0));
        }

        private List<Post> RetrievePosts(int begin)
        {
            List<Tag> tagsSelected = new List<Tag>();
            foreach (ListViewItem lvi in lvTags.Items)
                if (lvi.IsSelected) tagsSelected.Add((Tag)(int.Parse(lvi.Name.Substring(1))-1));
            return DBConn.GetPosts(begin, tagsSelected);
        }

        private void ShowPosts(List<Post> posts)
        {
            foreach (Post p in posts)
            {
                User u = DBConn.GetUser(p.UID);
                
                //create xaml elements to represent posts
                Grid container = new Grid();
                container.Background = new SolidColorBrush(Colors.Red);
                container.Margin = new Thickness(0, 10, 0, 0);
                RowDefinition rd50 = new RowDefinition(); rd50.Height = new GridLength(50);
                RowDefinition rd502 = new RowDefinition(); rd502.Height = new GridLength(50);
                RowDefinition rd100 = new RowDefinition(); rd100.Height = new GridLength(100);
                container.RowDefinitions.Add(rd50);
                container.RowDefinitions.Add(rd502);
                container.RowDefinitions.Add(rd100);

                StackPanel userDisplay = new StackPanel();
                userDisplay.Orientation = Orientation.Horizontal;
                container.Children.Add(userDisplay);

                Image userImage = new Image();
                userImage.Source = new BitmapImage(new Uri(this.BaseUri, Directory.GetCurrentDirectory() + @"\Assets\bet.png"));
                userDisplay.Children.Add(userImage);

                TextBlock userText = new TextBlock() { Text=u.Username };
                userDisplay.Children.Add(userText);

                TextBlock datePosted = new TextBlock() { Text = "Posted: " + p.Posted.ToString("MM/dd/yyyy") + " at " + p.Posted.ToString("HH:mm:ss"), HorizontalAlignment = HorizontalAlignment.Right, TextWrapping = TextWrapping.Wrap, Width = 140 };
                container.Children.Add(datePosted);

                TextBlock postTitle = new TextBlock() { Text = p.Title, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 26 };
                container.Children.Add(postTitle);
                Grid.SetRow(postTitle, 1);

                TextBlock postDescription = new TextBlock() { Text = p.Text, TextWrapping = TextWrapping.Wrap, FontSize = 16 };
                container.Children.Add(postDescription);
                Grid.SetRow(postDescription, 2);

                spFeed.Children.Add(container);
                /*<Grid Background="Red" Margin="0,10,0,0">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="50" />
                        <RowDefinition Height="50" />
                        <RowDefinition Height="100" />
                    </Grid.RowDefinitions>
                    <StackPanel Orientation="Horizontal">
                        <Image Source="Assets/githubfigurine-1024x539.jpg" />
                        <TextBlock Text="This is a User" VerticalAlignment="Center" />
                    </StackPanel>
                    <TextBlock Text="Posted: MM/DD/YYYY at HH:MM:SS" HorizontalAlignment="Right" TextWrapping="Wrap" Width="140" />
                    <TextBlock Text="This is a Title" HorizontalAlignment="Center" VerticalAlignment="Center" FontSize="26" Grid.Row="1" />
                    <TextBlock Text="This is a Description" TextWrapping="Wrap" FontSize="16" Grid.Row="2" />
                </Grid>*/
            }
        }

        private void BtnPost_Click(object sender, RoutedEventArgs e)
        {
            List<Tag> tagsSelected = new List<Tag>();
            foreach (ListViewItem pvi in pvTags.Items)
                if (pvi.IsSelected) tagsSelected.Add((Tag)(int.Parse(pvi.Name.Substring(1)) - 1));
            if (tbPostTitle.Text != string.Empty
                && tbPostText.Text != string.Empty)
            {
                DBConn.SubmitPost(user.UID, tbPostTitle.Text, tbPostText.Text, tagsSelected);
                spFeed.Children.Clear();
                ShowPosts(RetrievePosts(0));
            }
        }

        private void BtnTags_Click(object sender, RoutedEventArgs e)
        {
            tbNewTags.Text = string.Empty;
            popChooseTags.IsOpen = true;
        }

        private void BtnSelectTags_Click(object sender, RoutedEventArgs e)
        {
            List<Tag> tagsSelected = new List<Tag>();
            foreach (ListViewItem pvi in pvTags.Items)
                if (pvi.IsSelected) tagsSelected.Add((Tag)(int.Parse(pvi.Name.Substring(1)) - 1));
            foreach (Tag t in tagsSelected)
                tbNewTags.Text += t.ToString() + ", ";
            tbNewTags.Text = tbNewTags.Text.Remove(tbNewTags.Text.Length - 2);
            popChooseTags.IsOpen = false;
        }
    }
}
