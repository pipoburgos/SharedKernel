using SharedKernel.Application.Reporting;
using SharedKernel.Infrastructure.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SharedKernel.Integration.Tests.Reporting
{
    public class ReportRendererTests
    {
        [Fact]
        public void TestReportNotFound()
        {
            var service = new ReportRenderer(null);
            const string path = "random path.rdlc";
            const ExportReportType extension = ExportReportType.Pdf;

            Assert.Throws<FileNotFoundException>(() =>
            {
                service.RenderRdlc(path, extension);
            });
        }

        [Fact]
        public void TestReport()
        {
            var service = new ReportRenderer(null);
            var path = $"{Directory.GetCurrentDirectory()}/Reporting/BillExample.rdlc";
            const ExportReportType extension = ExportReportType.Pdf;
            var pathResult = $"{Directory.GetCurrentDirectory()}/Reporting/BillExample{Guid.NewGuid()}.pdf";

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
            var dataSources = new Dictionary<string, object>
            {
                {"ConceptsDataSet", billReportData.Concepts}
            };

            var bytes = service.RenderRdlc(path, extension, parameters, dataSources);

            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 78_000);
            Assert.True(bytes.Length < 79_000);
            File.WriteAllBytes(pathResult, bytes);
        }
    }
}
