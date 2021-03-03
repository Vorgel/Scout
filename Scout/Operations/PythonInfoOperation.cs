using Scout.Services;
using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace Scout.Operations
{
    public class PythonInfoOperation : IOperation
    {
        private readonly OutputProvider outputProvider;
        private readonly string OutputFileName = "PythonInfoOperation.json";

        public PythonInfoOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }

        public async Task Run()
        {
            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                {
                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("PythonInfoOperation");

                    var pythonVersion = ApplicationData.Current.LocalSettings.Values["pythonVersion"] as string;

                    if (pythonVersion.EndsWith("\\r\\n"))
                    {
                        pythonVersion.Substring(pythonVersion.Length - 4);
                    }

                    await outputProvider.CreateJsonFile(pythonVersion, OutputFileName);
                }
            }
            catch (Exception e) when (e is SecurityException || e is IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }
    }
}
