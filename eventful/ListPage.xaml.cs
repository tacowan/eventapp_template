using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using eventful.ViewModels;

namespace eventful
{
    public partial class ListPage : PhoneApplicationPage
    {
        public ListPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            App.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            pbar.Visibility = Visibility.Collapsed;
        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel item = MainLongListSelector.SelectedItem as ItemViewModel;
            if (item == null)
                return;
            NavigationService.Navigate(new Uri("/EventDetail.xaml?id=" + item.ID, UriKind.Relative));
            MainLongListSelector.SelectedItem = null;
        }
    }
}