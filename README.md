# BlobStorageFileHelper
.NET Core 3 library for download and upload files from/to Azure Blob Storage

## Usage
This is a general use of this little and simple library.
You could use this library to get and send files between different Azure Blob Storages.

This sample code create a file content on fly and save it as file on Azure Blob Storage.
In this same sample, you will download a file in Azure Blob Storage to disk.

```csharp
public static void Main(string[] args)
{
    // Create the file content
    var message = "Hello World!";
    var streamFileToUpload = WriteTextToStream(message);

    // Assign the filename
    var filename = "myFile.txt";
    // Assign the connection string for the Azure Blob Storage
    var connectionString = "<BLOB_STORAGE_CONNECTION_STRING>";
    // Assign the container that will keep the file
    var container = "myContainer";

    // Create the Azure Blob Storage client
    var blobStorageClient = new BlobStorageFileUtilities.BlobStorageClient(connectionString, container);

    // Upload file to Azure Blob Storage
    var result = blobStorageClient.UploadAsync(filename, streamFileToUpload).GetAwaiter().GetResult();
    if (!result)
    { 
        Console.WriteLine($"ERROR: {blobStorageClient.ErrorDetails}");
    }
    else
    {
        Console.WriteLine($"File {filename} uploaded correctly!");
        // Download file from Azure Blob Storage
        var streamFileDownloaded = blobStorageClient.DownloadAsync(filename).GetAwaiter().GetResult();
        if (streamFileDownloaded == null)
        {
            Console.WriteLine($"ERROR: {blobStorageClient.ErrorDetails}");
        }
        else
        {
            // Save the file to disk
            SaveStreamToDisk(streamFileDownloaded, @$"C:\temp\{filename}");
            Console.WriteLine($"File {filename} downloaded correctly!");
        }
    }
}

private static Stream WriteTextToStream(string message)
{
    var stringInMemoryStream = new MemoryStream(UTF8Encoding.Default.GetBytes(message));
    stringInMemoryStream.Position = 0;

    return stringInMemoryStream;
}

private static void SaveStreamToDisk(Stream stream, string filePath)
{
    stream.Seek(0, SeekOrigin.Begin);
    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    {
        stream.CopyTo(fileStream);
    }
}
```
