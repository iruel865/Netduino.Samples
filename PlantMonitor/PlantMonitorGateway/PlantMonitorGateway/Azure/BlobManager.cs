using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.RetryPolicies;

namespace PlantMonitorGateway.Azure
{
    public static class BlobManager
    {        
        private const string CONTAINER_NAME = "plant-log-container";

        public static async Task Initialize()
        {
            CloudStorageAccount storageAccount = Helper.CreateStorageAccountFromConnectionString();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINER_NAME);
            var requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
            await container.CreateIfNotExistsAsync(requestOptions, null);
        }

        public static async Task UploadLogFileAsync()
        {
            CloudStorageAccount storageAccount = Helper.CreateStorageAccountFromConnectionString();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINER_NAME);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(App.LOG_FILE_NAME);
            blockBlob.Properties.ContentType = "application/json";
            await blockBlob.UploadFromFileAsync(App.LOG_FILE_NAME);
        }

        public static async Task DownloadLogFileAsync()
        {
            CloudStorageAccount storageAccount = Helper.CreateStorageAccountFromConnectionString();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINER_NAME);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(App.LOG_FILE_NAME);
            await blockBlob.DownloadToFileAsync(App.LOG_FILE_NAME, FileMode.Create);
        }
    }
}