using System;
using System.Data.SqlClient;
using Caliburn.Micro;
using Scout.Helpers;
using Windows.Storage;
using Windows.UI.Popups;

namespace Scout.ViewModels
{
    public class DatabaseViewModel : Screen
    {
        public string SqlServerInstance { get; set; }

        public string DatabaseName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DatabaseViewModel()
        {
            this.SqlServerInstance = Environment.MachineName;
        }

        public string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.InitialCatalog = this.DatabaseName;
            builder.DataSource = this.SqlServerInstance;
            builder.UserID = this.Login;
            builder.Password = this.Password;
            //builder.InitialCatalog = "McDonald";
            //builder.DataSource = @"DESKTOP-0NN337C\SQLEXPRESS";
            //builder.UserID = "sa";
            //builder.Password = "pass";
            //builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }
        public bool CanTest(string sqlServerInstance, string databaseName, string login, string password)
        {
            return !String.IsNullOrEmpty(sqlServerInstance) && !String.IsNullOrEmpty(databaseName) && !String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password);
        }

        public async void Test(string sqlServerInstance, string databaseName, string login, string password)
        {
            ApplicationData.Current.LocalSettings.Values["canConnect"] = "Cannot establish connection";
            DatabaseTester databaseTester = new DatabaseTester(GetConnectionString());
            string message = string.Empty;

            try
            {
                await databaseTester.CanEstablishConnection();
                System.Threading.Thread.Sleep(2000);
                var canConnect = ApplicationData.Current.LocalSettings.Values["canConnect"] as string;

                if (canConnect == "true")
                {
                    message = "Connected, ";

                    await databaseTester.IsDbNotOccupied();

                    System.Threading.Thread.Sleep(2000);

                    var IsTheOnlyConnection = ApplicationData.Current.LocalSettings.Values["isTheOnlyConnection"] as string;

                    if (IsTheOnlyConnection == "true")
                    {
                         message += "no other connections detected.";
                    }
                    else
                    {
                        message += "multiple database connections detected.";
                    }
                }
                else
                {
                    message = canConnect.ToString();
                }
            }
            catch (SqlException e)
            {
                message = e.Message;
            }

            var dialog = new MessageDialog(message);

            await dialog.ShowAsync();
        }
    }
}
