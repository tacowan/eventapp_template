using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using Newtonsoft.Json;
using eventful.ViewModels;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;


namespace eventful
{
    public partial class EventDetail : PhoneApplicationPage
    {
        private EventDetailModel viewmodel;
        public EventDetail()
        {
            InitializeComponent();
            viewmodel = new EventDetailModel();
            DataContext = viewmodel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id = NavigationContext.QueryString["id"];
            var isConnected = Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable;

            if (isConnected)
                LoadData(id);
        }

        async private void LoadData(string id)
        {
            var client = new HttpClient();
            string url = "http://" + App.mawssubdomain + ".azurewebsites.net/eventful/rest/events/get?id=" + id;
            try
            {
                string response = await client.GetStringAsync(url);
                pbar.Visibility = Visibility.Collapsed;
                firstItem.Visibility = Visibility.Visible;
                XDocument doc = XDocument.Parse(response);
                viewmodel.Images = new ObservableCollection<ItemViewModel>(
                    from image in doc.Root.Element("images").Elements("image")
                    select new ItemViewModel
                    {
                        LineOne = image.Element("medium").Element("url").Value
                    });
                viewmodel.Url = doc.Root.Element("url").Value;
                viewmodel.Title = doc.Root.Element("title").Value;
                viewmodel.Image = viewmodel.Images[0].LineOne;
                viewmodel.Price = doc.Root.Element("price").Value;
                //viewmodel.Latitude = doc.Root.Element("latitude");
                //viewmodel.Longitude = doc.Root.Element("longitude");
                viewmodel.Venue = doc.Root.Element("venue_name").Value;
                viewmodel.Time = doc.Root.Element("start_time").Value;
                viewmodel.Address = doc.Root.Element("address").Value;
                viewmodel.Region = doc.Root.Element("region").Value;
                viewmodel.City = doc.Root.Element("city").Value;


                viewmodel.Links = new ObservableCollection<ItemViewModel>(
                    from link in doc.Root.Element("links").Elements("link")
                    select new ItemViewModel
                    {
                        LineOne = link.Element("url").Value,
                        ExternalUrl = link.Element("url").Value,
                        LineTwo = link.Element("description").Value
                    });
            } catch (System.Net.Http.HttpRequestException e)
            {

            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            ItemViewModel item = lb.SelectedItem as ItemViewModel;
            if (item == null) return;
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(item.ExternalUrl, UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(viewmodel.Url, UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MapsTask t = new MapsTask();
            t.SearchTerm = viewmodel.Address + "," + viewmodel.City + ", " + viewmodel.Region;
            t.Show();
        }
    }
}