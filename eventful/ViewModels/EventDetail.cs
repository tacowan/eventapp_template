using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventful.ViewModels
{
    class EventDetailModel : INotifyPropertyChanged
    {


        private string _title;

        public string Title {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        private string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; NotifyPropertyChanged("Url"); }
        }

        private string _venue;

        public string Venue
        {
            get { return _venue; }
            set { _venue = value; NotifyPropertyChanged("Venue"); }
        }

        private string _price;

        public string Price
        {
            get { return _price; }
            set { _price = value; NotifyPropertyChanged("Price"); }
        }


        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _region;

        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private string _time;

        public string Time
        {
            get {
                DateTime t = DateTime.Parse(_time);
                return t.ToShortTimeString(); 
            }
            set { _time = value; }
        }

        public string Date
        {
            get
            {
                DateTime t = DateTime.Parse(_time);
                return t.ToLongDateString();
            }
  
        }



        private ObservableCollection<ItemViewModel> _images;
        public ObservableCollection<ItemViewModel> Images {
            get { return _images; }
            set { _images = value; NotifyPropertyChanged("Images"); }
        }

        private ObservableCollection<ItemViewModel> _links;
        public ObservableCollection<ItemViewModel> Links { get { return _links; } set { _links = value;  NotifyPropertyChanged("Links"); } }

        private string _image;
        public string Image
        {
            get {
                if (_image != null)
                    return _image.Replace("medium", "block250");
                else return _image; 
            }
            set { _image = value; NotifyPropertyChanged("Image"); }
 
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
