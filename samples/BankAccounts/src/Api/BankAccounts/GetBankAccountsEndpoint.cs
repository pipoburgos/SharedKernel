using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedKernel.Api.Binders;
using SharedKernel.Application.Cqrs.Queries.Contracts;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Bank accounts Controller. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts")]
    public class GetBankAccountsEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Gets bank accounts paged. </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///{
        ///    "pageOptions": {
        ///        "skip": 0,
        ///        "take": 50,
        ///        "orders": [{"field": "Id"}]
        ///    }
        ///}
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IPagedList<BankAccountItem>>> Handle(
            [ModelBinder(BinderType = typeof(GetBankAccountsModelBinder))][FromQuery] GetBankAccounts getBankAccounts,
            CancellationToken cancellationToken)
        {
            return OkTyped(await QueryBus.Ask(getBankAccounts, cancellationToken));
        }

        private class GetBankAccountsModelBinder : PageOptionsBinder, IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                    throw new ArgumentNullException(nameof(bindingContext));

                var result = new GetBankAccounts
                {
                    PageOptions = GetPagedOptions(bindingContext)
                };

                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
        }
    }
}
