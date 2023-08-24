namespace SharedKernel.Application.Reporting
{
    /// <summary>
    /// SQL Server Reporting Services report renderer (rdlc extension)
    /// </summary>
    public interface IReportRenderer
    {
        /// <summary>
        /// Render a rdlc (SQL Server Reporting Services)
        /// </summary>
        /// <param name="reportPath">Absolute file path</param>
        /// <param name="exportReportType">Export file extension</param>
        /// <param name="parameters">Report parameters</param>
        /// <param name="dataSources">Report datasources</param>
        /// <returns>Array file contents</returns>
        byte[] RenderRdlc(string reportPath, ExportReportType exportReportType,
            Dictionary<string, string> parameters = null, Dictionary<string, object> dataSources = null);
    }
}
