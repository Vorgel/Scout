using Caliburn.Micro;
using Scout.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Scout.ViewModels
{
    public class OperationsViewModel : Screen
    {
        private BindableCollection<OperationBinder> _operations = new BindableCollection<OperationBinder>();

        public BindableCollection<OperationBinder> Operations
        {
            get
            {
                return _operations;
            }
            set
            {
                if (_operations != value)
                {
                    _operations = value;
                }
            }
        }

        public OperationsViewModel()
        {
            this.Operations.Add(new OperationBinder("OSInfoOperation"));
            this.Operations.Add(new OperationBinder("ConfigFilesOperation"));
            this.Operations.Add(new OperationBinder("SQLServerInfoOperation"));
        }

        public List<string> GetCheckedOperationsNames()
        {
            List<string> checkedOperations = new List<string>();

            foreach (var operation in this.Operations)
            {
                if (operation.IsChecked && !checkedOperations.Contains(operation.Name))
                {
                    checkedOperations.Add(operation.Name);
                }
            }

           return checkedOperations;
        }
    }
}
