using System;

using Scout.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Scout.Views
{
    public sealed partial class DatabasePage : Page
    {
        public DatabasePage()
        {
            InitializeComponent();
        }

        private DatabaseViewModel ViewModel
        {
            get { return DataContext as DatabaseViewModel; }
        }
    }
}
