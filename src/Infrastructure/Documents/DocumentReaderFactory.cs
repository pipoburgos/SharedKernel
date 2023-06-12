using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Documents
{
    /// <summary>  </summary>
    public class DocumentReaderFactory : IDocumentReaderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>  </summary>
        public DocumentReaderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>  </summary>
        public IDocumentReader Create(string name)
        {
            var providers = _serviceProvider.GetServices<IDocumentReader>().ToDictionary(e => e.Extension.ToLower());

            var extension = name;
            if (name.Contains('.'))
                extension = name.Split('.').Last().ToLower();

            if (!providers.ContainsKey(extension))
                throw new NotImplementedException($"Document reader '{extension}' no implemented.");

            return providers[name];
        }

        /// <summary>  </summary>
        public IEnumerable<string> GetExtensions()
        {
            return _serviceProvider.GetServices<IDocumentReader>().Select(e => e.Extension.ToLower());
        }
    }
}
