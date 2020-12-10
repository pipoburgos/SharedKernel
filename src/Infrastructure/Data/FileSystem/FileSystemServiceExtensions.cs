using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.FileSystem.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.FileSystem
{
    public static class FileSystemServiceExtensions
    {
        public static IServiceCollection AddFileSystemUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IFileSystemUnitOfWorkAsync, FileSystemUnitOfWork>()
                .AddTransient<IDirectoryRepositoryAsync, DirectoryRepositoryAsync>()
                .AddTransient<IFileRepositoryAsync, FileRepositoryAsync>();
        }
    }
}
