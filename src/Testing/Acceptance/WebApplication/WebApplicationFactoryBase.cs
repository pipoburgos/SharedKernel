using Castle.Core.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using NSubstitute;
using SharedKernel.Application.Events;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.Events.Synchronous;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using File = System.IO.File;
using Path = System.IO.Path;

namespace SharedKernel.Testing.Acceptance.WebApplication
{
    public class WebApplicationFactoryBase<TStartup, TContext> : WebApplicationFactory<TStartup>
        where TStartup : class where TContext : DbContext
    {
        private const string Environment = "Testing";
        private bool _firstTime = true;
        private DatabaseManager _dataBase;

        public DateTime? DateTime { get; set; }

        public override async ValueTask DisposeAsync()
        {
            if (_dataBase != default)
            {
                await _dataBase.DisposeAsync();
                _dataBase = null;
            }

            await base.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            const string ficheroConfiguracion = $"appsettings.{Environment}.json";
            if (!File.Exists(ficheroConfiguracion))
                throw new FileNotFoundException(ficheroConfiguracion);

            builder
                .UseEnvironment(Environment)
                .ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddJsonFile("appsettings.json");
                    conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), ficheroConfiguracion), false);
                })
                .ConfigureServices(x =>
                {
                    IdentityModelEventSource.ShowPII = true;

                    //x.AddAuthentication(options =>
                    //{
                    //    options.DefaultScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    //    options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    //    options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    //}).AddFakeJwtBearer();

                    x.RemoveAll<IEmailSender>().AddTransient(_ => Substitute.For<IEmailSender>());

                    x.RemoveAll<IDateTime>().AddTransient(_ =>
                    {
                        var dateTime = Substitute.For<IDateTime>();
                        dateTime.UtcNow.Returns(DateTime ?? System.DateTime.UtcNow);
                        return dateTime;
                    });
                })
                // En medio se ejecutan los servicios de la app
                .ConfigureTestServices(services =>
                {
                    //services
                    //    .RemoveAll<IHttpClientFactory>()
                    //    .AddTransient(_ =>
                    //    {
                    //        var array = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

                    //        var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
                    //        mockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                    //            .Returns(new HttpResponseMessage(HttpStatusCode.OK)
                    //            { Content = new ByteArrayContent(array) });

                    //        var httpClientFactory = Substitute.For<IHttpClientFactory>();
                    //        var httpClient = new HttpClient(mockHttpMessageHandler);
                    //        httpClient.BaseAddress = new Uri("http://example.com");
                    //        httpClientFactory.CreateClient("SSRS").Returns(httpClient);

                    //        return httpClientFactory;
                    //    });

                    services
                        .RemoveAll<IEventBus>()
                        .AddSingleton<IEventBus, SynchronousEventBus>();
                });
        }

        internal new async Task<HttpClient> CreateClient()
        {
            if (_firstTime)
            {
                await RegenerateDatabase(CancellationToken.None);
                _firstTime = false;
            }

            if (_dataBase != default)
                await _dataBase.DisposeAsync();

            _dataBase = new DatabaseManager(GetNewContext());

            var client = base.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            client.DefaultRequestHeaders.Add("Accept-Language", "es-ES");
            return client;
        }

        protected virtual async Task RegenerateDatabase(CancellationToken cancellationToken)
        {
            await Task.Delay(15_000, cancellationToken);
            var unitOfWork = Services.GetRequiredService<TContext>();
            //await unitOfWork.Database.EnsureDeletedAsync(cancellationToken);
            unitOfWork.Database.SetCommandTimeout(300);
            await unitOfWork.Database.MigrateAsync(cancellationToken);
        }

        internal DatabaseManager Database() => _dataBase;

        internal TContext GetNewContext()
        {
            return Services.CreateScope().ServiceProvider.GetService<TContext>();
        }


    }
}