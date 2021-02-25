using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using Scout.Operations;
using Scout.Services;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace Scout.ViewModels
{
    public class MainViewModel : Screen
    {
        // TODO: sprawdz autofaca w caliburn
        // bo oeprations view model moze przyjmowac liste operacji

        private string _chosenDirectory = string.Empty;

        private OutputProvider outputProvider;
        public OperationsViewModel OperationsViewModel { get; set; }
        private IEnumerable<IOperation> operations;

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
            this.outputProvider = new OutputProvider();

            AutofacConfig autofacConfig = new AutofacConfig(this.outputProvider);
            this.operations = autofacConfig.GetContainer();

           var checkedOperationsNames = this.OperationsViewModel.GetCheckedOperationsNames();

            foreach (var operation in this.operations)
            {
                var operationName = GetOperationName(operation.ToString());

                if (checkedOperationsNames.Contains(operationName))
                {
                    operation.Run();
                }
            }

            var dialog = new MessageDialog(string.Join(",",checkedOperationsNames));
            await dialog.ShowAsync();
        }

        private string GetOperationName(string name)
        {
            return name.Split('.').Last();
        }

        public async void PickDirectory()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            var path = await folderPicker.PickSingleFolderAsync();

            if (path != null)
            {
                this.ChosenDirectory = path.Path;
            }
        }
    }
}
