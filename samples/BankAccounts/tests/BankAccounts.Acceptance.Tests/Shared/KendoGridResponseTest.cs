namespace BankAccounts.Acceptance.Tests.Shared
{
    public class KendoGridResponseTest<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
    }
}
