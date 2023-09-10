﻿using Azure.Storage;
using Azure.Storage.Blobs;
using Reenbit_Test.Server;

namespace Reenbit_Test.Services
{
    public class AzureBlobService
    {
        private readonly string _storageAccount = "tresbienblob";
        private readonly string _accessKey = "2Nzto2cu59r0/haG/zfLcFEynP59+5ySFUcwByNflGSRbCYWZtTDiLFQz3PlzkYOEsiDhBwZkseq+ASttjF6/Q==";
        private readonly BlobContainerClient _filesContainer;

        public AzureBlobService()
        {
            var credantial = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credantial);
            _filesContainer = blobServiceClient.GetBlobContainerClient("blobcontainer");
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            List<BlobDto> files = new List<BlobDto>();

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<BlobResponceDto> UploadAsync(IFormFile blob)
        {
            BlobResponceDto responce = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            responce.Status = $"File {blob.FileName} Uploaded Successfully";
            responce.Error = false;
            responce.Blob.Uri = client.Uri.AbsoluteUri;
            responce.Blob.Name = client.Name;

            return responce;
        }

        public async Task<BlobDto?> DownloadAsync(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;
                string contentType = content.Value.Details.ContentType;

                return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
            }
            return null;
        }

        public async Task<BlobResponceDto> DeleteAsync(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            await file.DeleteAsync();

            return new BlobResponceDto { Error = false, Status = $"File: {blobFilename} has been successfuly deleted." };
        }
    }
}
