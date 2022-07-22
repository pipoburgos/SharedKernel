using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Validators;
using SharedKernel.Integration.Tests.Cqrs.Commands;
using SharedKernel.Integration.Tests.Shared;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Middlewares
{
    public class MiddlewaresWithResponseTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddValidators(typeof(SampleCommandWithResponseHandler))
                .AddValidationMiddleware()
                .AddInMemoryCommandBus();
        }

        [Fact]
        public async Task TestCommandHandlerWithResponse()
        {
            const int value = 15;
            var request = new SampleCommandWithResponse(value);

            var response = await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);

            response.Should().Be(value);
        }

        [Fact]
        public async Task TestCommandHandlerWithResponseValidationError()
        {
            var request = new SampleCommandWithResponse(default);
            await Assert.ThrowsAsync<ValidationFailureException>(() =>
                GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None));
        }
    }
}
