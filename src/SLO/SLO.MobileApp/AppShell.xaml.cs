using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SLO.MobileApp.Core.Brokers.Storages;
using System.IO;

namespace SLO.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            string currentDatabaseFilePath =
                Path.Combine(
                    FileSystem.AppDataDirectory,
                    StorageBroker.CURRENT_DATABASE_FILE_NAME);

            if (File.Exists(path: currentDatabaseFilePath))
            {
                return;
            }

            bool databaseFileAssetExists =
                await FileSystem.AppPackageFileExistsAsync(
                    filename: StorageBroker.CURRENT_DATABASE_FILE_NAME);

            if (databaseFileAssetExists is false)
            {
                return;
            }

            using Stream assetStream =
                await FileSystem.OpenAppPackageFileAsync(
                    filename: StorageBroker.CURRENT_DATABASE_FILE_NAME);

            using FileStream fileStream =
                File.Create(path: currentDatabaseFilePath);

            await assetStream.CopyToAsync(fileStream);
        }
    }
}
