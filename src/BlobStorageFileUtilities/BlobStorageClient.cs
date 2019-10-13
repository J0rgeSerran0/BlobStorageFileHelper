using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageFileUtilities
{
    public class BlobStorageClient
    {
        private readonly CloudBlobClient _cloudBlobClient = null;
        private readonly CloudBlobContainer _blobStorageContainer = null;

        private readonly string _container = null;

        public string ErrorDetails = String.Empty;

        public BlobStorageClient(string blobStorageConnectionString, string container)
        {
            if (String.IsNullOrEmpty(blobStorageConnectionString)) throw new ArgumentException($"{nameof(blobStorageConnectionString)} is null or empty");
            if (String.IsNullOrEmpty(container)) throw new ArgumentException($"{nameof(container)} is null or empty");

            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(blobStorageConnectionString, out storageAccount))
            {
                _cloudBlobClient = storageAccount.CreateCloudBlobClient();
            }
            else
            {
                throw new Exception("The connection string is not valid. An error has been returned. Check it and try again.");
            }

            _container = container;
            _blobStorageContainer = GetContainerReference(_container);
        }

        private CloudBlobContainer GetContainerReference(string container)
        {
            if (String.IsNullOrEmpty(container)) throw new ArgumentException($"{nameof(container)} is null or empty");

            return _cloudBlobClient.GetContainerReference(container);
        }

        public async Task<Stream> DownloadAsync(string filename)
        {
            try
            {
                Stream stream = new MemoryStream();

                var cloudBlockBlob = _blobStorageContainer.GetBlockBlobReference(filename);
                
                await cloudBlockBlob.DownloadToStreamAsync(stream);

                return stream;
            }
            catch (Exception ex)
            {
                ErrorDetails = ex.Message;

                return null;
            }
        }

        public async Task<bool> UploadAsync(string filename, Stream stream)
        {
            try
            {
                ErrorDetails = String.Empty;

                var cloudBlockBlob = _blobStorageContainer.GetBlockBlobReference(filename);

                await cloudBlockBlob.UploadFromStreamAsync(stream);

                return true;
            }
            catch (Exception ex)
            {
                ErrorDetails = ex.Message;

                return false;
            }
        }

    }
}