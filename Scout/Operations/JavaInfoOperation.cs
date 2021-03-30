using Scout.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace Scout.Operations
{
    public class JavaInfoOperation : IOperation
    {
        private readonly OutputProvider outputProvider;
        private readonly string OutputFileName = "JavaInfoOperation.json";

        public JavaInfoOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }

        public async Task Run()
        {
            try
             {
                 if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                 {
                     var command = "(Get-Command java | Select-Object -ExpandProperty Version).toString()";
                     var outputPath = Path.Combine(outputProvider.OutputDirectory, OutputFileName);

                     ApplicationData.Current.LocalSettings.Values["parameters"] = command + "," + outputPath;

                     await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("JavaInfoOperation");

                    var javaVersion = ApplicationData.Current.LocalSettings.Values["javaVersion"] as string;

                    await outputProvider.CreateJsonFile(javaVersion, OutputFileName);
                 }
             }
             catch (Exception ex)
             {
                 throw;
             }
        }
    }
}
