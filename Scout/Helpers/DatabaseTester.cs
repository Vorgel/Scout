using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace Scout.Helpers
{
    public class DatabaseTester
    {
        private string connectionString;

        public DatabaseTester(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CanEstablishConnection()
        {
            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                {
                    ApplicationData.Current.LocalSettings.Values["connectionString"] = this.connectionString;

                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CanConnect");
                }
            }
            catch (Exception e) when (e is SecurityException || e is IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }

        public async Task IsDbNotOccupied()
        {
            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                {
                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("IsTheOnlyConnection");
                }
            }
            catch (Exception e) when (e is SecurityException || e is IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }       
    }
}
