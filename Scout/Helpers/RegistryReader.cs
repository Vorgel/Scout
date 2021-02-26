using Microsoft.Win32;
using System.Collections.Generic;

namespace Scout.Helpers
{
    public static class RegistryReader
    {
        public  static List<RegistryRead> ReadRegistries(string registryKey, List<string> keyNames)
        {
            List<RegistryRead> registries = new List<RegistryRead>();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    return null;
                }

                foreach (var keyName in keyNames)
                {
                    var keyValue = key.GetValue(keyName);

                    RegistryRead registry;

                    if (keyValue != null)
                    {
                        registry = new RegistryRead { Name = keyName, Value = keyValue.ToString() };
                    }
                    else
                    {
                        registry = new RegistryRead { Name = keyName, Value = "null" };
                    }

                    registries.Add(registry);
                }

                return registries;
            }
        }
    }
}
