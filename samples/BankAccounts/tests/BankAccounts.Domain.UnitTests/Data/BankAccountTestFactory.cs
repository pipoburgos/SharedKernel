﻿using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Events;
using System;

namespace BankAccounts.Domain.UnitTests.Data
{
    public static class BankAccountTestFactory
    {
        public static BankAccount Create(Guid? id = default, InternationalBankAccountNumber iban = default,
            User owner = default, Movement initialMovement = default)
        {
            var bankAccount = new BankAccount(id ?? Guid.NewGuid(),
                iban ?? InternationalBankAccountNumberTestFactory.Create(), owner ?? UserTestFactory.Create(),
                initialMovement ?? MovementTestFactory.Create(), DateTime.Now);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }
    }
}