using BankAccounts.Domain.Shared;
using SharedKernel.Application.Cqrs.Middlewares;
using System;

namespace BankAccounts.Application.Shared
{
    internal class BankAccountRetryPolicyExceptionHandler : IRetryPolicyExceptionHandler
    {
        /// <summary>Check if an exception has to be retried</summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool NeedToRetryTheException(Exception exception) =>
            exception.GetType() != typeof(BankAccountBaseException);
    }
}
