using Caliburn.Micro;
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
        private bool _isOSInfoOperationChecked;

        public bool IsOSInfoOperationChecked
        {
            get
            {
                return _isOSInfoOperationChecked;
            }

            set
            {
                if (_isOSInfoOperationChecked != value)
                {
                     _isOSInfoOperationChecked = value;
                    NotifyOfPropertyChange(() => IsOSInfoOperationChecked);
                }
            }
        }

        public bool IsSQLServerInfoOperationChecked { get; set; }

        public bool IsConfigFilesOperationChecked { get; set; }

        List<string> checkedOperations;

        public OperationsViewModel()
        {
        }

        public void  CreateCheckedOperationsArray()
        {
            List<string> checkedOperations = new List<string>();

            if (this.IsOSInfoOperationChecked && !checkedOperations.Contains("OSInfoOperation"))
            {
                checkedOperations.Add("OSInfoOperation");
            }

            if (this.IsSQLServerInfoOperationChecked && !checkedOperations.Contains("SQLServerInfoOperation"))
            {
                checkedOperations.Add("SQLServerInfoOperation");
            }

            if (this.IsConfigFilesOperationChecked && !checkedOperations.Contains("ConfigFilesOperation"))
            {
                checkedOperations.Add("ConfigFilesOperation");
            }

            foreach (var operation in checkedOperations)
            {
                Debug.WriteLine(operation);
            }

            this.checkedOperations = checkedOperations;
        }
    }
}
