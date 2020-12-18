using AspNetCore.Reporting;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Reporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Infrastructure.Reporting
{
    /// <summary>
    /// SQL Server Reporting Services report renderer (rdlc extension)
    /// </summary>
    public class ReportRenderer : IReportRenderer
    {
        private readonly ICustomLogger<ReportRenderer> _logger;

        public ReportRenderer(
            ICustomLogger<ReportRenderer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Render a rdlc (SQL Server Reporting Services)
        /// </summary>
        /// <param name="reportPath">Absolute file path</param>
        /// <param name="exportReportType">Export file extension</param>
        /// <param name="parameters">Report parameters</param>
        /// <param name="dataSources">Report datasources</param>
        /// <returns>Array file contents</returns>
        public byte[] RenderRdlc(string reportPath, ExportReportType exportReportType, Dictionary<string, string> parameters = null,
            Dictionary<string, object> dataSources = null)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var report = new LocalReport(reportPath);

                if (dataSources != default)
                {
                    foreach (var dataSource in dataSources)
                    {
                        report.AddDataSource(dataSource.Key, dataSource.Value);
                    }
                }

                var result = report.Execute((RenderType)exportReportType, 1, parameters);

                return result.MainStream;
            }
            catch (Exception e)
            {
                _logger?.Error(e, e.Message);
                throw;
            }
        }
    }
}
