using System;
using System.IO;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Scout.Services
{
    public class OutputProvider
    {
        public OutputProvider()
        {
            this.SetDirectoryName();
            this.CreateDirectory();
        }

        public string DirectoryName { get; private set; }

        public async void CreateDirectory()
        {
            if (!Directory.Exists(this.DirectoryName))
            {
                try
                {
                    StorageFolder newfolder = await DownloadsFolder.CreateFolderAsync(this.DirectoryName);
                }
                catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException || e is DirectoryNotFoundException)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void CreateJsonFile(object objectToSerialize, string fileName)
        {
            var fullPath = Path.Combine(this.DirectoryName, fileName);

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Include,
            };

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(sw, objectToSerialize);
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException || e is IOException || e is System.Security.SecurityException)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private void SetDirectoryName()
        {
            string localDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss", System.Globalization.CultureInfo.InvariantCulture);
            this.DirectoryName = $"Scout_Result_{localDate}";
        }
    }
}
