using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Scout.Helpers
{
    public static class RegistryReader
    {
        public static string [] ReadSubkeyNames(string registryKey)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    return null;
                }

                return key.GetSubKeyNames();
            }
        }

        public  static List<RegistryRead> ReadRegistries(string registryKey)
        {
            List<RegistryRead> registries = new List<RegistryRead>();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    return null;
                }

               var keyNames = key.GetValueNames();

                foreach (var keyName in keyNames)
                {
                    try
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
                    catch (Exception e )
                    {
                        throw;
                    }
                }

                return registries; 
            }
        }

        public static RegistryRead ReadRegistry(string registryKey, string keyName)
        {
            RegistryRead registry = new RegistryRead();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    return null;
                }

                try
                {
                    var keyValue = key.GetValue(keyName);

                    if (keyValue != null)
                    {
                        registry.Name = keyName;
                        registry.Value = keyValue.ToString();
                    }
                    else
                    {
                        registry.Name = keyName;
                        registry.Value = "null";
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                return registry;
            }
        }

        public static List<RegistryRead> ReadRegistries(string registryKey, string [] keyNames)
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
                    try
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
                    catch (Exception e)
                    {
                        throw;
                    }
                }

                return registries;
            }
        }
    }
}
