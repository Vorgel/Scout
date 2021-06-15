using Ionic.Zip;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
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

                    case "/passwordZipFiles":
                    {
                        ZipFilesWithPassword();
                        break;
                    }

                    case "/canConnect":
                    {
                        CanEstablishConnection();
                        break;
                    }

                    case "/isTheOnlyConnection":
                    {
                        IsTheOnlyConnection();
                        break;
                    }

                }
            }
         }

        private static void ZipFiles()
        {
            try
            {
                var directoryToZip = ApplicationData.Current.LocalSettings.Values["outputPath"] as string;

                string zipPath = directoryToZip + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(directoryToZip);
                    zip.Save(zipPath);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static void ZipFilesWithPassword()
        {
            try
            {
                var password = ApplicationData.Current.LocalSettings.Values["zipPassword"] as string;

                var directoryToZip = ApplicationData.Current.LocalSettings.Values["outputPath"] as string;

                string zipPath = directoryToZip + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.Password = password;
                    zip.AddDirectory(directoryToZip);
                    zip.Save(zipPath);
                }
            }
            catch (Exception e)
            {
                throw;
            }
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

        private static void IsTheOnlyConnection()
        {
            var connectionString = ApplicationData.Current.LocalSettings.Values["connectionString"] as string;

            try
            {
                int dbID = GetDbID(connectionString);

                string message = string.Empty;

                if (dbID < 0)
                {
                    message = "false";
                }

                string getExistingDbConnections = $"SELECT COUNT(*) FROM sys.sysprocesses WHERE dbid='{dbID}' AND (status='runnable' OR status='sleeping' OR status='running' OR status='suspended')";

                int rows = (int)GetSqlQueryResult(getExistingDbConnections, connectionString);

                message = rows == 1 ? "true" : "false";

                ApplicationData.Current.LocalSettings.Values["isTheOnlyConnection"] = message;
            }
            catch (SqlException e)
            {
                ApplicationData.Current.LocalSettings.Values["isTheOnlyConnection"] = e.Message;
            }
        }

        private static object GetSqlQueryResult(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return reader[0];
                    }
                }
            }

            return null;
        }

        private static int GetDbID(string connectionString)
        {
            var databaseID = GetSqlQueryResult("SELECT DB_ID() AS [Database ID]", connectionString);

            return databaseID == null ? -1 : Int32.Parse(databaseID.ToString());
        }

        private static void CanEstablishConnection()
        {
            var connectionString = ApplicationData.Current.LocalSettings.Values["connectionString"] as string;
            var message = string.Empty;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        message = "true";
                    }
                    else
                    {
                        message = "false";
                    }    

                    ApplicationData.Current.LocalSettings.Values["canConnect"] = message;
                }
            }
            catch (SqlException e)
            {
                ApplicationData.Current.LocalSettings.Values["canConnect"] = e.Message;
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
