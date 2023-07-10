using SharedKernel.Domain.RailwayOrientedProgramming;

namespace BankAccounts.Domain.BankAccounts
{
    public class InternationalBankAccountNumber : ValueObject<InternationalBankAccountNumber>
    {
        protected InternationalBankAccountNumber() { }

        protected InternationalBankAccountNumber(string countryCheckDigit, string entityCode, string officeNumber,
            string controlDigit, string accountNumber)
        {
            CountryCheckDigit = countryCheckDigit;
            EntityCode = entityCode;
            OfficeNumber = officeNumber;
            ControlDigit = controlDigit;
            AccountNumber = accountNumber;
        }

        public static Result<InternationalBankAccountNumber> Create(string countryCheckDigit, string entityCode,
            string officeNumber, string controlDigit, string accountNumber)
        {
            var iban = new InternationalBankAccountNumber(countryCheckDigit, entityCode, officeNumber, controlDigit,
                accountNumber);

            return Result.Create(iban)
                .EnsureAppendError(
                    e => string.IsNullOrWhiteSpace(e.CountryCheckDigit),
                    $"'{nameof(CountryCheckDigit)}' must not be empty.")
                .EnsureAppendError(
                    e => string.IsNullOrWhiteSpace(e.EntityCode),
                    $"'{nameof(EntityCode)}' must not be empty.")
                .EnsureAppendError(
                    e => string.IsNullOrWhiteSpace(e.OfficeNumber),
                    $"'{nameof(OfficeNumber)}' must not be empty.")
                .EnsureAppendError(
                    e => string.IsNullOrWhiteSpace(e.ControlDigit),
                    $"'{nameof(ControlDigit)}' must not be empty.")
                .EnsureAppendError(
                    e => string.IsNullOrWhiteSpace(e.AccountNumber),
                    $"'{nameof(AccountNumber)}' must not be empty.");
        }

        public string CountryCheckDigit { get; private set; }

        public string EntityCode { get; private set; }

        public string OfficeNumber { get; private set; }

        public string ControlDigit { get; private set; }

        public string AccountNumber { get; private set; }
    }
}
