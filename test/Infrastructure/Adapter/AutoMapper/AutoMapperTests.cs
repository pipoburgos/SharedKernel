using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Mapper;
using SharedKernel.Infrastructure.AutoMapper;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Adapter.AutoMapper
{
    public partial class AutoMapperTests : InfrastructureTestCase<FakeStartup>
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddAutoMapperSharedKernel(typeof(MyProfile).Assembly);
        }

        [Fact]
        public void MapppingOk()
        {
            MapperFactory.SetCurrent(GetRequiredService<IMapperFactory>());
            var emails = new List<string> { "a@a.es", "b@b.es" };
            var source = new DocumentSource { Name = "Say may name", Emails = emails };
            var result = source.MapTo<DocumentTarget>();
            result.Name.Should().Be(source.Name);
            result.Emails.Should().Equal(source.Emails,
                (o1, o2) => string.Compare(o1, o2, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
