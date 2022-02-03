using SharedKernel.Domain.ValueObjects;

namespace BankAccounts.Domain.BankAccounts
{
    public class InternationalBankAccountNumber : ValueObject<InternationalBankAccountNumber>
    {
        protected InternationalBankAccountNumber() { }

        public InternationalBankAccountNumber(string countryCheckDigit, string entityCode, string officeNumber,
            string controlDigit, string accountNumber)
        {
            CountryCheckDigit = countryCheckDigit;
            EntityCode = entityCode;
            OfficeNumber = officeNumber;
            ControlDigit = controlDigit;
            AccountNumber = accountNumber;
        }

        public string CountryCheckDigit { get; private set; }

        public string EntityCode { get; private set; }

        public string OfficeNumber { get; private set; }

        public string ControlDigit { get; private set; }

        public string AccountNumber { get; private set; }
    }
}
