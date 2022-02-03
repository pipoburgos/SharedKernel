using System;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    public static class MovementFactory
    {
        public static Movement CreateMovement(Guid id, string concept, decimal quantity, DateTime date)
        {
            return new Movement(id, concept, quantity, date);
        }
    }
}
