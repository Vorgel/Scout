using Caliburn.Micro;
using Scout.Helpers;
using Scout.Operations;
using System.Collections.Generic;

/*TODO:
 * - Serilog
 * - determine SQL Server version in SQLServerInfoOperation
 *  - powershellinfo
 *  - installed java
 *  - instaleld python
 *  
 * - find out other operation ideas 
 * - introduce custom save place
 * - introduce databaseverifier
 */


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
            this.Operations.Add(new OperationBinder("OSInfoOperation", "Get OS informations"));
            this.Operations.Add(new OperationBinder("ConfigFilesOperation", "Get .config files"));
            this.Operations.Add(new OperationBinder("LogFilesOperation", "Get .log files"));
            this.Operations.Add(new OperationBinder("AccessDatabasesOperation", "Get .el files"));
            this.Operations.Add(new OperationBinder("PowershellInfoOperation", "Get Powershell informations"));
            this.Operations.Add(new OperationBinder("PythonInfoOperation", "Get Python version"));
            this.Operations.Add(new OperationBinder("JavaInfoOperation", "Get Java version"));
        }

        public List<string> GetCheckedOperationsNames()
        {
            List<string> checkedOperations = new List<string>();

            foreach (var operation in this.Operations)
            {
                if (operation.IsChecked && !checkedOperations.Contains(operation.OperationName))
                {
                    checkedOperations.Add(operation.OperationName);
                }
            }

           return checkedOperations;
        }
    }
}
