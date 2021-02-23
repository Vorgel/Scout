using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Scout.Operations
{
    class OSInfoOperation : IOperation
    {
        public async void Run()
        {
            MessageDialog dialog = new MessageDialog("dupa");

            await dialog.ShowAsync();
        }
    }
}
