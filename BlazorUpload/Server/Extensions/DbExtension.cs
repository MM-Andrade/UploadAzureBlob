using BlazorUpload.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorUpload.Server.Extensions
{
    public static class DbExtension
    {
        public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectStringName = "UploadAzureBlobDBConn";
            var connectionStringDb = configuration.GetConnectionString(connectStringName);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionStringDb);
            });


        }
    }
}
