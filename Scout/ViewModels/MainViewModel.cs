using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Caliburn.Micro;
using Scout.Operations;
using Scout.Services;
using Windows.Storage.Pickers;

namespace Scout.ViewModels
{
    public class MainViewModel : Screen
    {
        private string _chosenDirectory = string.Empty;

        private OutputProvider outputProvider;
        private OperationsViewModel operationsViewModel;
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

        public MainViewModel()
        {
            this.operationsViewModel = new OperationsViewModel();
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

            this.operationsViewModel.CreateCheckedOperationsArray();

            foreach (var operation in this.operations)
            {
                Debug.WriteLine(operation.GetType()); 
                Debug.WriteLine(operation.ToString());
                operation.Run();
            }
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
