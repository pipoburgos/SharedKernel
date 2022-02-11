using BankAccounts.Domain.BankAccounts;
using System;

namespace BankAccounts.Domain.UnitTests.Data
{
    public static class MovementTestFactory
    {
        public static Movement Create(Guid? id = default, decimal? amount = default)
        {
            return new Movement(id ?? Guid.NewGuid(), "Conce", amount ?? 23, new DateTime(2020, 3, 5));
        }
    }
}
