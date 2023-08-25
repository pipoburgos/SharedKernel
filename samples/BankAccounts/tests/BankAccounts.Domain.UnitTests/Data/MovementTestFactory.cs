using BankAccounts.Domain.BankAccounts;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace BankAccounts.Domain.Tests.Data;

internal static class MovementTestFactory
{
    public static Result<Movement> Create(Guid? id = default, decimal? amount = default)
    {
        return Movement.Create(id ?? Guid.NewGuid(), "Conce", amount ?? 23, new DateTime(2020, 3, 5));
    }
}
