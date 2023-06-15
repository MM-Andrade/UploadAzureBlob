using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorUpload.Shared.Model;

namespace BlazorUpload.Server.Repositories
{
    public class AzureStorage : IAzureStorage
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("Azure:BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("Azure:BlobContainerName");
            _logger = logger;
        }

        public async Task<BlobResponse> DeleteAsync(string blobFilename)
        {
            BlobContainerClient blobContainer = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient file = blobContainer.GetBlobClient(blobFilename);

            try
            {
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponse { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            return new BlobResponse { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
        }

        public async Task<BlobDTO> DownloadAsync(string blobFilename)
        {
            BlobContainerClient blobContainer = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                BlobClient file = blobContainer.GetBlobClient(blobFilename);

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();

                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    return new BlobDTO { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                 when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFilename} was not found.");
            }

            return null;
        }

        public async Task<List<BlobDTO>> ListAsync()
        {
            BlobContainerClient blobContainer = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            List<BlobDTO> files = new List<BlobDTO>();

            await foreach (BlobItem file in blobContainer.GetBlobsAsync())
            {
                string uri = blobContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDTO
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });

            }
            return files;
        }

        public async Task<BlobResponse> UploadAsync(IFormFile file)
        {
            BlobResponse response = new();

            BlobContainerClient blobContainer = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                BlobClient client = blobContainer.GetBlobClient(file.FileName);

                await using (Stream? data = file.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                response.Status = $"File {file.FileName} uploaded successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {file.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {file.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }

            catch (RequestFailedException ex)
            {
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            return response;

        }
    }
}
