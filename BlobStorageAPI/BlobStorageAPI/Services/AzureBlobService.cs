using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace BlobStorageAPI.Services
{
    public class AzureBlobService
    {
        BlobServiceClient _blobClient;
        private readonly IConfiguration _configuration;
        BlobContainerClient _containerClient;
        public AzureBlobService(IConfiguration Configuration)
        {  
            _configuration = Configuration;
            _blobClient = new BlobServiceClient(_configuration.GetSection("BlobStorgaeInfo:ConnectionStrings").Value.ToString());
            _containerClient = _blobClient.GetBlobContainerClient(_configuration.GetSection("BlobStorgaeInfo:StorageName").Value.ToString());
        }

        public async Task<List<Azure.Response<BlobContentInfo>>> UploadFiles(IFormFile file)
        {

            var azureResponse = new List<Azure.Response<BlobContentInfo>>();

            string fileName = file.FileName;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var client = await _containerClient.UploadBlobAsync(fileName, memoryStream, default);
                azureResponse.Add(client);
            }
            return azureResponse;
        }

        public async Task<List<BlobItem>> GetUploadedBlobs()
        {
            var items = new List<BlobItem>();
            var uploadedFiles = _containerClient.GetBlobsAsync();
            await foreach (BlobItem file in uploadedFiles)
            {
                items.Add(file);
            }

            return items;
        }
        public async Task<CloudBlob> ReturnBlobsFile(string fileName)
        {
            if (CloudStorageAccount.TryParse(_configuration.GetSection("BlobStorgaeInfo:ConnectionStrings").Value.ToString(), out CloudStorageAccount storageAccount))
            {
                CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = BlobClient.GetContainerReference(_configuration.GetSection("BlobStorgaeInfo:StorageName").Value.ToString());

                if (await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference(fileName);
                    return file;
                }
            }
            return null;
        }
    }
}

