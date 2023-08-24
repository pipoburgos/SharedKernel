namespace SharedKernel.Domain.Tests.Shared
{
    public static class Repeater
    {
        private static IEnumerable<T> Repeat<T>(Func<T> method, int quantity)
        {
            if(method == null)
                throw new ArgumentNullException(nameof(method));

            return Enumerable.Repeat(method(), quantity);
        }

        public static IEnumerable<T> RepeateLessThan<T>(Func<T> method, int max)
        {
            return Repeat(method, IntegerMother.LessThan(max));
        }

        public static IEnumerable<T> Random<T>(Func<T> method)
        {
            return Repeat(method, IntegerMother.LessThan(5));
        }
    }
}