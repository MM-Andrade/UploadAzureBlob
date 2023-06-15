using BlazorUpload.Server.Repositories;

namespace BlazorUpload.Server.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFileToUploadRepository, FileToUploadRepository>();
            services.AddScoped<IAzureStorage, AzureStorage>();
        }
    }
}
