namespace BankAccounts.Acceptance.Tests.Shared
{
    public class KendoGridResponseTest<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public int Total { get; set; }
    }
}
