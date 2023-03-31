namespace BankAccounts.Domain.BankAccounts.Specifications
{
    internal class AtLeast18YearsOldSpec : ISpecification<User>
    {
        private readonly DateTime _date;

        public AtLeast18YearsOldSpec(DateTime date)
        {
            _date = date.Date;
        }

        public Expression<Func<User, bool>> SatisfiedBy()
        {
            return user => _date.DayOfYear < user.Birthdate.DayOfYear
                ? _date.Year - user.Birthdate.Year - 1 >= 18
                : _date.Year - user.Birthdate.Year >= 18;
        }
    }
}
