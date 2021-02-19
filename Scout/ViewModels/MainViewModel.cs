using System;
using System.Windows.Input;
using Caliburn.Micro;
using GalaSoft.MvvmLight.Command;
using Scout.Helpers;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace Scout.ViewModels
{
    public class MainViewModel : Screen
    {
        private string _chosenDirectory = string.Empty;
        public string ChosenDirectory
        {
            get
            {
                return _chosenDirectory;
            }
            set
            {
                if ( _chosenDirectory != value)
                {
                    _chosenDirectory = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ChosenDirectory"));
                }
            }

        }

        public RelayCommand PickDirectoryCommand => new RelayCommand(this.PickDirectoryAsync);

        public RelayCommand RunCommand => new RelayCommand(this.Run);

        public MainViewModel()
        {

        }

        public async void Run()
        {
            var dialog = new MessageDialog($"{ChosenDirectory}");
            await dialog.ShowAsync();
        }

        public async void PickDirectoryAsync()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            var path = await folderPicker.PickSingleFolderAsync();
            this.ChosenDirectory = path.Path;
        }
    }
}
