using Scout.Helpers;
using Scout.Services;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace Scout.Operations
{
    class PowershellInfoOperation : IOperation
    {
        private readonly OutputProvider outputProvider;
        private readonly string OutputFileName = "PowershellInfoOperation";

        public PowershellInfoOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }
        public async Task Run()
        {
            try
            {
                string powershellRegistry = @"SOFTWARE\Microsoft\PowerShell";

                var subKeys = RegistryReader.ReadSubkeyNames(powershellRegistry);

                foreach (var subKey in subKeys)
                {
                    string installRegistry = $@"{powershellRegistry}\{subKey}";

                    if (IsPowershellInstalled(installRegistry))
                    {
                        string powershellEngineRegistry = $@"{installRegistry}\PowerShellEngine";

                        var registries = RegistryReader.ReadRegistries(powershellEngineRegistry);

                        var outputFileName = this.OutputFileName + subKey;

                        await this.outputProvider.CreateJsonFile(registries, outputFileName);
                    }
                }
            }
            catch (Exception e) when (e is SecurityException || e is System.IO.IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }

        private bool IsPowershellInstalled(string powershellRegistryPath)
        {
            try
            {
                var registry = RegistryReader.ReadRegistry(powershellRegistryPath, "Install");

                if (registry != null)
                {
                    var isInstalledInt = Convert.ToInt32(registry.Value);

                    return Convert.ToBoolean(isInstalledInt);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e) when (e is SecurityException || e is System.IO.IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }
    }
}
