using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eventful.Resources;
using System.Xml.Linq;
using System.Linq;
using System.Net.Http;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Net.NetworkInformation;

namespace eventful.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<ItemViewModel> Venues { get; private set; }
        public ObservableCollection<ItemViewModel> Query { get; private set; }

        public ItemViewModel Selected;

        private string _sampleProperty = "";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string ContextTitle
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("ContextTitle");
                }
            }
        }

        private bool _network = true;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public bool Network
        {
            get
            {
                return _network;
            }
            set
            {
                if (value != _network)
                {
                    _network = value;
                    NotifyPropertyChanged("Network");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            var isConnected = Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable;

            

            if (isConnected)
            {
                // do stuff that talks to the interwebseses here...

                LoadData2();
                try
                {
                    var client = new HttpClient();
                    string response = await client.GetStringAsync(App.MY_BLOB_CONTAINER + "topvenues");
                    XDocument xmlVenues = XDocument.Parse(response);
                    Venues = new ObservableCollection<ItemViewModel>(
                        from venue in xmlVenues.Root.Element("venues").Elements("venue")
                        select new ItemViewModel
                        {
                            LineOne = venue.Element("name").Value,
                            LineTwo = venue.Element("description").Value,
                            LineThree = venue.Element("image").Element("medium") == null ? null : venue.Element("image").Element("medium").Element("url").Value,
                            ExternalUrl = venue.Element("url").Value,
                            ID = venue.Attribute("id").Value
                        });
                    NotifyPropertyChanged("Venues");
                    this.IsDataLoaded = true;
                }
                catch (System.Net.Http.HttpRequestException e)
                {
                    Network = false;
                }
            }
        }

        public async void LoadData2()
        {
            var client = new HttpClient();
            try
            {
                string response = await client.GetStringAsync(App.MY_BLOB_CONTAINER + "popular");
                XDocument events = XDocument.Parse(response);
                Items = new ObservableCollection<ItemViewModel>(
                    from venue in events.Root.Element("events").Elements("event")
                    select new ItemViewModel
                    {
                        LineOne = venue.Element("title").Value,
                        LineTwo = venue.Element("venue_name").Value,
                        LineThree = venue.Element("image").Element("medium").Element("url").Value,
                        ExternalUrl = venue.Element("url").Value,
                        ID = venue.Attribute("id").Value
                    });
                NotifyPropertyChanged("Items");
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Network = false;
            }
        }

        public async void LoadQuery(string url)
        {
            var isConnected = Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable;


            if (isConnected)
            {
                if (Query != null)
                    Query.Clear();
                var client = new HttpClient();
                try
                {
                    string response = await client.GetStringAsync(url);
                    XDocument xmlVenues = XDocument.Parse(response);
                    Query = new ObservableCollection<ItemViewModel>(
                        from item in xmlVenues.Root.Element("events").Elements("event")
                        where item.Element("image").Element("medium") != null
                        select new ItemViewModel
                        {
                            LineOne = item.Element("title").Value,
                            LineTwo = item.Element("venue_name").Value,
                            LineThree = item.Element("image").Element("medium").Element("url").Value,
                            ExternalUrl = item.Element("url").Value,
                            ID = item.Attribute("id").Value
                        });

                    NotifyPropertyChanged("Query");
                }
                catch (System.Net.Http.HttpRequestException e)
                {
                    Network = false;
                }
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}