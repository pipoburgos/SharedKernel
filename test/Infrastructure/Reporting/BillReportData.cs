using System.Collections.Generic;

namespace SharedKernel.Infraestructure.Tests.Reporting
{
    public class BillReportData
    {
        public BillReportData()
        {
            Concepts = new List<BillReportConcept>();
        }

        public bool IsUser { get; set; }

        public string Number { get; set; }

        public string DateString { get; set; }

        public string Address { get; set; }

        public string FiscalName { get; set; }

        public string Cif { get; set; }

        public string IvaAmount { get; set; }

        public string TaxableString { get; set; }

        public string TotalString { get; set; }

        public IEnumerable<BillReportConcept> Concepts { get; set; }
    }
}