using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using eventful.ViewModels;
using Microsoft.Phone.Tasks;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;


namespace eventful
{

    public partial class MainPage : PhoneApplicationPage
    {
 
        //
        // Example category URL, creates the blob
        // http://yoursubdomain.azurewebsites.net/cache/rest/events/search?location=dallas&sort_order=popularity&page_size=30&category=sports&blobname=sports
        // http://yoursubdomain.azurewebsites.net/cache/rest/events/search?location=dallas&sort_order=popularity&page_size=30&category=family_fun_kids&blobname=family_fun_kids
        // http://yoursubdomain.azurewebsites.net/cache/rest/venues/search?sort_order=popularity&location=dallas&page_size=20&blobname=topvenues
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            App.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!"Network".Equals(e.PropertyName))
                return;
            nonetwork.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Collapsed;

        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            var isConnected = Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable;

            if (!isConnected)
            {
                nonetwork.Visibility = Visibility.Visible;
                menu.Visibility = Visibility.Collapsed;

            }

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();              
            }
            HubTileService.UnfreezeGroup("foo1");
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.ContextTitle = "Concerts";
            App.ViewModel.LoadQuery(App.MY_BLOB_CONTAINER + "music");
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.ContextTitle = "Family";

            App.ViewModel.LoadQuery(App.MY_BLOB_CONTAINER + "family_fun_kids");
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap3(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.ContextTitle = "Sports";

            App.ViewModel.LoadQuery(App.MY_BLOB_CONTAINER + "sports");
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap4(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.ContextTitle = "Performing Arts";
            App.ViewModel.LoadQuery(App.MY_BLOB_CONTAINER + "performing_arts");
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            // If selected item is null (no selection) do nothing
            if (selector.SelectedItem == null)
                return;
            ItemViewModel item = MainLongListSelector.SelectedItem as ItemViewModel;
            App.ViewModel.ContextTitle = item.LineOne;
            App.ViewModel.LoadQuery("http://" + App.mawssubdomain + ".azurewebsites.net/eventful/rest/events/search?sort_order=popularity&page_size=50&location=" + item.ID);
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
            selector.SelectedItem = null;
        }

        private void listboxDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedIndex == -1)
                return;
            ItemViewModel Selected  =  lb.SelectedItem as ItemViewModel;          
            NavigationService.Navigate(new Uri("/EventDetail.xaml?id=" + Selected.ID, UriKind.Relative));
            lb.SelectedIndex = -1;
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.Network = true;
            App.ViewModel.LoadData();
            menu.Visibility = Visibility.Visible;
            nonetwork.Visibility = Visibility.Collapsed;
        }
    }
}