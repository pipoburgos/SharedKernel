using FluentAssertions;
using SharedKernel.Application.Reporting;
using SharedKernel.Infrastructure.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SharedKernel.Integration.Tests.Reporting;

public class ReportRendererTests
{
    [Fact]
    public void TestReportNotFound()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var service = new ReportRenderer(null);
        const string path = "random path.rdlc";
        const ExportReportType extension = ExportReportType.Pdf;

        var func = () => service.RenderRdlc(path, extension);

        func.Should().Throw<FileNotFoundException>();
    }

    //[Fact]
    public void TestReport()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var service = new ReportRenderer(null);
        const string path = "Reporting/BillExampleReport.rdlc";
        const ExportReportType extension = ExportReportType.Pdf;

        var billReportData = new BillReportData
        {
            IsUser = true,
            Number = "23",
            FiscalName = "tax",
            DateString = DateTime.Now.ToShortDateString(),
            Cif = "cif",
            Address = "address",
            TaxableString = "tax",
            IvaAmount = "iva",
            TotalString = "total"
        };

        var parameters = new Dictionary<string, string>
        {
            {"IsUserParameter", billReportData.IsUser.ToString()},
            {"NumberParameter", billReportData.Number},
            {"DateParameter", billReportData.DateString},
            {"FiscalNameParameter", billReportData.FiscalName},
            {"CifParameter", billReportData.Cif},
            {"AddressParameter", billReportData.Address ?? "-"},
            {"TotalTaxableParameter", billReportData.TaxableString},
            {"IvaPercentageParameter", billReportData.IvaAmount},
            {"TotalAmountParameter", billReportData.TotalString}
        };

        var dataSources = new Dictionary<string, object> { { "ConceptsDataSet", billReportData.Concepts } };

        var bytes = service.RenderRdlc(path, extension, parameters, dataSources);

        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThanOrEqualTo(78_000);
        bytes.Length.Should().BeLessThanOrEqualTo(79_000);
    }
}
