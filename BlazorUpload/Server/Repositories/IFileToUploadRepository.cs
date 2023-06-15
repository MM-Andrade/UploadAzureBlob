using BlazorUpload.Shared.Model;

namespace BlazorUpload.Server.Repositories
{
    public interface IFileToUploadRepository
    {
        Task<IEnumerable<FileToUpload>> GetAll();
        Task<FileToUpload> GetByIdAsync(Guid id);
        Task<FileToUpload> AddAsync(FileToUpload fileToUpload);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
