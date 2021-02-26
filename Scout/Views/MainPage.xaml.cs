using Scout.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Scout.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(new OperationsViewModel());
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
