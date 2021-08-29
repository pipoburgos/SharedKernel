using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Adapter.AutoMapper;
using SharedKernel.Integration.Tests.Shared;
using SharedKernel.Application.Adapter;
using Xunit;

namespace SharedKernel.Integration.Tests.Adapter.AutoMapper
{
    public partial class AutoMapperTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddAutoMapperSharedKernel(typeof(MyProfile).Assembly);
        }

        [Fact]
        public void MapppingOk()
        {
            TypeAdapterFactory.SetCurrent(GetRequiredService<ITypeAdapterFactory>());
            var emails = new List<string> {"a@a.es", "b@b.es"};
            var source = new DocumentSource {Name = "Say may name", Emails = emails};
            var result = source.MapTo<DocumentTarget>();
            result.Name.Should().Be(source.Name);
            result.Emails.Should().Equal(source.Emails,
                (o1, o2) => string.Compare(o1, o2, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
