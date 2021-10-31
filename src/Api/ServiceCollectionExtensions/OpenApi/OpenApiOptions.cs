using System.Collections.Generic;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi
{
    /// <summary>
    /// Open api options
    /// </summary>
    public class OpenApiOptions
    {
        /// <summary>
        /// Open api info title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Open api name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// De Url of swagger.json. Default: "swagger/v1/swagger.json"
        /// </summary>
        public string Url { get; set; } = "swagger/v1/swagger.json";

        /// <summary>
        /// Documentation file name
        /// </summary>
        public string XmlDocumentationFile { get; set; }

        /// <summary>
        /// Collapse actions of controllers
        /// </summary>
        public bool Collapsed { get; set; } = true;

        /// <summary>
        /// Documentation files names
        /// </summary>
        public IEnumerable<string> XmlDocumentationFiles { get; set; }
    }
}