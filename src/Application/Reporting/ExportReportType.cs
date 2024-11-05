namespace SharedKernel.Application.Reporting;

/// <summary>
/// Output file extension for SQL Server Reporting Services render
/// </summary>
public enum ExportReportType
{
    /// <summary>
    /// word 2003-2007 .doc
    /// </summary>
    Word = 0,

    /// <summary>
    /// word 2010-2016 .docx
    /// </summary>
    WordOpenXml = 1,

    /// <summary>
    /// excel 2003-2007 .xls
    /// </summary>
    Excel = 2,

    /// <summary>
    /// excel 2010-2016 .xlsx
    /// </summary>
    ExcelOpenXml = 3,

    /// <summary>
    /// pdf file
    /// </summary>
    Pdf = 4,

    /// <summary>
    /// image
    /// </summary>
    Image = 5,

    /// <summary>
    /// html5
    /// </summary>
    Html = 6,

    /// <summary>
    /// RPL
    /// </summary>
    Rpl = 7,
}