using Microsoft.Win32;
using Scout.Helpers;
using Scout.Services;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace Scout.Operations
{
    class OSInfoOperation : IOperation
    {
        private const string OutputFileName = "OSInfoOperation.json";

        private readonly OutputProvider outputProvider;
        private readonly List<string> keyNames = new List<string>
        {
            "ProductName",
            "EditionID",
            "PathName",
            "CurrentVersion",
            "ReleaseId",
            "BuildBranch",
            "BuildLabEx",
            "BuildLab",
            "InstallationType",
        };

        public OSInfoOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }

        public async Task Run()
        {
            try
            {
                string registryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

                List<RegistryRead> registries = RegistryReader.ReadRegistries(registryKey, this.keyNames);

                if (registries != null)
                {
                   await this.outputProvider.CreateJsonFile(registries, OutputFileName);
                }
                else
                {
                    return;
                }
            }
            catch (Exception e) when (e is SecurityException || e is System.IO.IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }
    }
}
