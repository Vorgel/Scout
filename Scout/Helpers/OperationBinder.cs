using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Helpers
{
    public class OperationBinder : Screen
    {
        private bool _isChecked;
        public string Name { get; set; }

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

        public OperationBinder(string name)
        {
            this.Name = name;
        }
    }
}
