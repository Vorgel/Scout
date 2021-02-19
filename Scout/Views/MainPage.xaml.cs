using System;
using Scout.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace Scout.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
