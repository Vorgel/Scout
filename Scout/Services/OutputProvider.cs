using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Search;

namespace Scout.Services
{
    public class OutputProvider
    {
        public string OutputDirectory { get; private set; }
        public StorageFolder SearchedDirectory { get; set; }

        public StorageFolder folder;

        public OutputProvider(StorageFolder searchedDirectory)
        {
            this.SearchedDirectory = searchedDirectory;
        }

        public async Task Setup()
        {
            this.SetDirectoryName();
            await CreateTemporaryDirectory();
        }

        public async Task CreateTemporaryDirectory()
        {
            if (!Directory.Exists(this.OutputDirectory))
            {
                try
                {
                    StorageFolder newfolder = await DownloadsFolder.CreateFolderAsync(this.OutputDirectory, CreationCollisionOption.GenerateUniqueName);
                    Debug.WriteLine("folder created");

                    this.folder = newfolder;
                }
                catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException || e is DirectoryNotFoundException)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public async Task DeleteTemporaryDirectory()
        {
            try
            {
                await folder.DeleteAsync();
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException || e is DirectoryNotFoundException)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task CreateZipFile()
        {
            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                {
                    ApplicationData.Current.LocalSettings.Values["outputPath"] = folder.Path;

                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("ZipFiles");
                }
            }
            catch (Exception e) when (e is SecurityException || e is IOException || e is UnauthorizedAccessException)
            {
                throw;
            }
        }

        public async Task CreateJsonFile(object objectToSerialize, string fileName)
        {            
            string json = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);

            try
            {
                StorageFile storageFile = await folder.CreateFileAsync(fileName);

                await FileIO.WriteTextAsync(storageFile, json);
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException || e is IOException || e is System.Security.SecurityException)
            {
                throw;
            }
        }

        private void SetDirectoryName()
        {
            string localDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss", System.Globalization.CultureInfo.InvariantCulture);
            this.OutputDirectory = $"Scout_Result_{localDate}";
        }

        public async  Task GetAppFiles(string searchKey)
        {
            try
            {
                List<string> fileTypeFilter = new List<string>
                {
                    searchKey
                };

                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);

                StorageFileQueryResult queryResult = this.SearchedDirectory.CreateFileQueryWithOptions(queryOptions);

                var files = await queryResult.GetFilesAsync();

                foreach (var file in files)
                {
                    var copyTo = Path.Combine(this.OutputDirectory, file.Name);
                    await file.CopyAsync(this.folder, file.DisplayName, NameCollisionOption.ReplaceExisting);
                }
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is ArgumentException)
            {
                throw;
            }
        }
    }
}
