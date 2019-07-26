using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlantMonitorApp.Azure
{
    public static class BlobManager
    {
        private const string CONNECTION_STRING = "[INSERT CONNECTION STRING]";
        private const string CONTAINER_NAME = "plant-log-container";

        public static async Task DownloadLogFileAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CONNECTION_STRING);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINER_NAME);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(App.LOG_FILE_NAME);
            await blockBlob.DownloadToFileAsync(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + App.LOG_FILE_NAME, FileMode.Create);
        }
    }
}