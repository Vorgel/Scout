using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Scout.Operations;
using Scout.Services;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace Scout.ViewModels
{
    public class MainViewModel : Screen
    {
        private string _chosenDirectory;

        private OutputProvider outputProvider;
        private IEnumerable<IOperation> operations;
        public OperationsViewModel OperationsViewModel { get; set; }
        public string ZipPassword { get; set; }

        public StorageFolder ChosenDirectoryStorage { get; set; }

        public string ChosenDirectory
        {
            get
            {
                return _chosenDirectory;
            }

            set
            {
                if (_chosenDirectory != value)
                {
                    _chosenDirectory = value;
                    NotifyOfPropertyChange(() => ChosenDirectory);
                }
            }
        }

        public MainViewModel(OperationsViewModel operationsViewModel)
        {
            this.OperationsViewModel = operationsViewModel ;
        }

        public bool CanRun(string chosenDirectory)
        {
            return !string.IsNullOrEmpty(chosenDirectory);
        }

        public async void Run(string chosenDirectory)
        {
            this.outputProvider = new OutputProvider(this.ChosenDirectoryStorage);

            await this.outputProvider.Setup();

            AutofacConfig autofacConfig = new AutofacConfig(this.outputProvider);
            this.operations = autofacConfig.GetContainer();

           var checkedOperationsNames = this.OperationsViewModel.GetCheckedOperationsNames();

            foreach (var operation in this.operations)
            {
                var operationName = GetOperationName(operation.ToString());

                if (checkedOperationsNames.Contains(operationName))
                {
                    await operation.Run();
                }
            }

            if (!String.IsNullOrEmpty(this.ZipPassword))
            {
                await this.outputProvider.CreateSecuredZipFile(this.ZipPassword);
            }
            else
            {
                await this.outputProvider.CreateZipFile();
            }

            MessageDialog messageDialog = new MessageDialog("Zip file created.");
            await messageDialog.ShowAsync();
        }

        private string GetOperationName(string name)
        {
            return name.Split('.').Last();
        }

        public async void PickDirectory()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder path = await folderPicker.PickSingleFolderAsync();

            this.ChosenDirectoryStorage = path;

            if (path != null)
            {
                this.ChosenDirectory = path.Path;
            }
        }
    }
}
