namespace SharedKernel.Integration.Tests.Reporting
{
    public class BillReportData
    {
        public BillReportData()
        {
            Concepts = new List<BillReportConcept>();
        }

        public bool IsUser { get; set; }

        public string Number { get; set; } = null!;

        public string DateString { get; set; } = null!;

        public string? Address { get; set; }

        public string FiscalName { get; set; } = null!;

        public string Cif { get; set; } = null!;

        public string IvaAmount { get; set; } = null!;

        public string TaxableString { get; set; } = null!;

        public string TotalString { get; set; } = null!;

        public IEnumerable<BillReportConcept> Concepts { get; set; }
    }
}