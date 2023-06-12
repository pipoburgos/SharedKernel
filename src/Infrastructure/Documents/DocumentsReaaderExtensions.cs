using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Csv;
using SharedKernel.Infrastructure.Documents.Database.DotNetDbf;
using SharedKernel.Infrastructure.Documents.Excel.Npoi;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Documents
{
    /// <summary>  </summary>
    public static class DocumentsReaaderExtensions
    {
        /// <summary>  </summary>
        public static IServiceCollection AddDocumentReaderFactory(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] assemblies)
        {
            return services
                .AddTransient<ICsvReader, CsvReader>()
                .AddTransient<IDatabaseReader, DotNetDatabaseReader>()
                .AddTransient<IExcelReader, NpoiExcelReader>()
                .AddTransient<IDocumentReaderFactory, DocumentReaderFactory>()
                .AddFromMatchingInterface<IDocumentReader>(serviceLifetime,
                    assemblies ?? new[] { typeof(DocumentReaderFactory).Assembly });
        }
    }
}
