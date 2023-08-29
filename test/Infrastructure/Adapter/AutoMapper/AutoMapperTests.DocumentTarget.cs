namespace SharedKernel.Integration.Tests.Adapter.AutoMapper
{
    public partial class AutoMapperTests
    {
        internal class DocumentTarget
        {
            public string Name { get; set; } = null!;

            public IEnumerable<string> Emails { get; set; } = null!;
        }
    }
}