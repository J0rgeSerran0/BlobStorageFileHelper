using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace BlobStorageFileUtilities
{
    public class BlobStorageClient
    {
        private readonly CloudBlobClient _cloudBlobClient = null;

        public BlobStorageClient(string blobStorageConnectionString)
        {
            if (String.IsNullOrEmpty(blobStorageConnectionString)) throw new ArgumentException($"{nameof(blobStorageConnectionString)} is null or empty");

            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(blobStorageConnectionString, out storageAccount))
            {
                _cloudBlobClient = storageAccount.CreateCloudBlobClient();
            }
            else
            {
                throw new Exception("The connection string is not valid. An error has been returned. Check it and try again.");
            }
        }

        public async Task<CloudBlobContainer> GetContainerReferenceAsync(string container)
        {
            if (String.IsNullOrEmpty(container)) throw new ArgumentException($"{nameof(container)} is null or empty");

            return await Task.Run(() => _cloudBlobClient.GetContainerReference(container));
        }

    }
}
