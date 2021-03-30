using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Windows.Storage;

namespace Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
            // parameters: command, outputPath

            if (args.Length > 2)
            {
                switch (args[2])
                {
                    case "/pythonInfoOperation":
                    { 
                        GetPythonVersion();
                        break;
                    }

                    case "/javaInfoOperation":
                    {
                        GetJavaVersion();
                        break;
                    }

                    case "/zipFiles":
                    {
                        ZipFiles();
                        break;
                    }
                }
            }
         }
        private static void ZipFiles()
        {
            var directoryToZip = ApplicationData.Current.LocalSettings.Values["outputPath"] as string;
            string zipPath = directoryToZip + ".zip";

            ZipFile.CreateFromDirectory(directoryToZip, zipPath,CompressionLevel.Optimal, false);
        }

        private static void GetJavaVersion()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "java.exe",
                    Arguments = " -version",
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                Process pr = Process.Start(psi);

                ApplicationData.Current.LocalSettings.Values["javaVersion"] = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception is " + ex.Message);
                throw;
            }
        }

        private static void GetPythonVersion()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "python.exe",
                    Arguments = " --version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                using (Process pr = Process.Start(psi))
                {
                    using (StreamReader reader = pr.StandardOutput)
                    {
                        string result = reader.ReadLine();
                        ApplicationData.Current.LocalSettings.Values["pythonVersion"] = result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception is " + ex.Message);
                throw;
            }
         }
    }
}
