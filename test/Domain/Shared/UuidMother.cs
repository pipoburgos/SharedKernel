namespace SharedKernel.Domain.Tests.Shared
{
    public static class UuidMother
    {
        public static string Random()
        {
            return Guid.NewGuid().ToString();
        }
    }
}