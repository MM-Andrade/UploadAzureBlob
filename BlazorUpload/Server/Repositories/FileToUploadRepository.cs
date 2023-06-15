using BlazorUpload.Server.Data;
using BlazorUpload.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorUpload.Server.Repositories
{
    public class FileToUploadRepository : IFileToUploadRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public FileToUploadRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<FileToUpload> AddAsync(FileToUpload fileToUpload)
        {
            var newFile = _applicationDbContext.Files.Add(fileToUpload).Entity;
            await _applicationDbContext.SaveChangesAsync();

            return newFile;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var file = await GetByIdAsync(id);

            if (file is null) return false;
            _applicationDbContext.Files.Remove(file);

            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<FileToUpload>> GetAll()
        {
            var files = await _applicationDbContext.Files.ToListAsync();
            return files;
        }

        public async Task<FileToUpload> GetByIdAsync(Guid id)
        {
            var file = await _applicationDbContext.Files.SingleOrDefaultAsync(p => p.Id == id);
            return file;
        }
    }
}
