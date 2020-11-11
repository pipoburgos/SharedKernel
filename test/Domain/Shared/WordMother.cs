namespace SharedKernel.Domain.Tests.Shared
{
    public static class WordMother
    {
        public static string Random()
        {
            return MotherCreator.Random().Word();
        }
    }
}