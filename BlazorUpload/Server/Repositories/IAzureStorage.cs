using BlazorUpload.Shared.Model;

namespace BlazorUpload.Server.Repositories
{
    public interface IAzureStorage
    {
        Task<BlobResponse> UploadAsync(IFormFile file);
        Task<BlobDTO> DownloadAsync(string blobFilename);
        Task<BlobResponse> DeleteAsync(string blobFilename);
        Task<List<BlobDTO>> ListAsync();
    }
}
