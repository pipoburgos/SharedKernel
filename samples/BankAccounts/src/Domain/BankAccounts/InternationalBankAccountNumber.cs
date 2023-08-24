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
            string officeNumber, string controlDigit, string accountNumber) =>
            Result.Create<InternationalBankAccountNumber>(default)
                .EnsureAppendError(
                    _ => !string.IsNullOrWhiteSpace(countryCheckDigit),
                    Error.Create($"'{nameof(CountryCheckDigit)}' must not be empty.", nameof(CountryCheckDigit)))
                .EnsureAppendError(
                    _ => !string.IsNullOrWhiteSpace(entityCode),
                    Error.Create($"'{nameof(EntityCode)}' must not be empty.", nameof(EntityCode)))
                .EnsureAppendError(
                    _ => !string.IsNullOrWhiteSpace(officeNumber),
                    Error.Create($"'{nameof(OfficeNumber)}' must not be empty.", nameof(OfficeNumber)))
                .EnsureAppendError(
                    _ => !string.IsNullOrWhiteSpace(controlDigit),
                    Error.Create($"'{nameof(ControlDigit)}' must not be empty.", nameof(ControlDigit)))
                .EnsureAppendError(
                    _ => !string.IsNullOrWhiteSpace(accountNumber),
                    Error.Create($"'{nameof(AccountNumber)}' must not be empty.", nameof(AccountNumber)))
                .Map(_ => new InternationalBankAccountNumber(countryCheckDigit, entityCode, officeNumber, controlDigit,
                    accountNumber));

        public string CountryCheckDigit { get; private set; }

        public string EntityCode { get; private set; }

        public string OfficeNumber { get; private set; }

        public string ControlDigit { get; private set; }

        public string AccountNumber { get; private set; }
    }
}
