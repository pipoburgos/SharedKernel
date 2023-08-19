using AspNetCore.Reporting;
using SharedKernel.Application.Reporting;
using System.Text;

namespace SharedKernel.Infrastructure.Reporting;

/// <summary> SQL Server Reporting Services report renderer (rdlc extension). </summary>
public class ReportRenderer : IReportRenderer
{
    /// <summary> Render a rdlc (SQL Server Reporting Services) </summary>
    /// <param name="reportPath">Absolute file path</param>
    /// <param name="exportReportType">Export file extension</param>
    /// <param name="parameters">Report parameters</param>
    /// <param name="dataSources">Report datasources</param>
    /// <returns>Array file contents</returns>
    public byte[] RenderRdlc(string reportPath, ExportReportType exportReportType,
        Dictionary<string, string>? parameters = default, Dictionary<string, object>? dataSources = default)
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
}
