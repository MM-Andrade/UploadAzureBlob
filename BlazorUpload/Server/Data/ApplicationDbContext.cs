using BlazorUpload.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorUpload.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {   
        }

        public DbSet<FileToUpload> Files { get; set; }
    }
}

