using Caliburn.Micro;
using Scout.Operations;

namespace Scout.Helpers
{
    public class OperationBinder : Screen
    {
        private bool _isChecked;
        public string OperationName { get; set; }

        public string DisplayDefinition { get; set; }
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    NotifyOfPropertyChange(() => IsChecked);
                }
            }
        }

        public OperationBinder(string name, string displayDefinition)
        {
            this.OperationName = name;
            this.DisplayDefinition = displayDefinition;
        }
    }
}
