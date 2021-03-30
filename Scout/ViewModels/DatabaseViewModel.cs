using System;
using System.Data.SqlClient;
using Caliburn.Micro;
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
            builder.Password = this.Password;
            builder.UserID = this.Login;

            return builder.ConnectionString;
        }

        public bool CanTest(string sqlServerInstance, string databaseName, string login, string password)
        {
            return !String.IsNullOrEmpty(sqlServerInstance) && !String.IsNullOrEmpty(databaseName) && !String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password);
        }

        public async void Test(string sqlServerInstance, string databaseName, string login, string password)
        {
            var dialog = new MessageDialog(GetConnectionString());
            await dialog.ShowAsync();
        }
    }
}
