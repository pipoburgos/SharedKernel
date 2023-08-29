using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Repositories.FileSystem;
using SharedKernel.Infrastructure.Data.FileSystem.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileSystemServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileSystemUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IFileSystemUnitOfWorkAsync, FileSystemUnitOfWork>()
                .AddTransient<IDirectoryRepositoryAsync, DirectoryRepositoryAsync>()
                .AddTransient<IFileRepositoryAsync, FileRepositoryAsync>();
        }
    }
}
