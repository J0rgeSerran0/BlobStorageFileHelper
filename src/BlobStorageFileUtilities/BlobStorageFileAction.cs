using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageFileUtilities
{
    public class BlobStorageFileAction
    {
        private readonly BlobStorageClient _blobStorageClient = null;
        private readonly string _container = null;

        private readonly CloudBlobContainer _blobStorageContainer = null;

        public BlobStorageFileAction(BlobStorageClient blobStorageClient, string container)
        {
            if (blobStorageClient == null) throw new ArgumentException($"{nameof(blobStorageClient)} is null or empty");
            if (String.IsNullOrEmpty(container)) throw new ArgumentException($"{nameof(container)} is null or empty");

            _blobStorageClient = blobStorageClient;
            _container = container;

            _blobStorageContainer = _blobStorageClient.GetContainerReferenceAsync(_container).GetAwaiter().GetResult();
        }

        public async Task<Stream> DownloadAsync(string filename)
        {
            Stream stream = new MemoryStream();

            var cloudBlockBlob = _blobStorageContainer.GetBlockBlobReference(filename);
            
            await cloudBlockBlob.DownloadToStreamAsync(stream);

            return stream;
        }

        public async Task UploadAsync(string filename, Stream stream)
        {
            var cloudBlockBlob = _blobStorageContainer.GetBlockBlobReference(filename);

            await cloudBlockBlob.UploadFromStreamAsync(stream);
        }

    }
}