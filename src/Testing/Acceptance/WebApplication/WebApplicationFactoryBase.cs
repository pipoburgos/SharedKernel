using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using File = System.IO.File;

namespace SharedKernel.Testing.Acceptance.WebApplication;

public abstract class WebApplicationFactoryBase<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private bool _firstTime = true;
    private DatabaseManager? _dataBase;

    public bool DeleteDatabase { get; set; } = true;

    public string Environment { get; set; } = "Testing";

    public string Culture { get; set; } = "en-US";

    public DateTime? DateTime { get; set; }

    public override async ValueTask DisposeAsync()
    {
        if (_dataBase != default)
        {
            await _dataBase.DisposeAsync().ConfigureAwait(false);
            _dataBase = null;
        }

        await base.DisposeAsync().ConfigureAwait(false);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var ficheroConfiguracion = $"appsettings.{Environment}.json";
        if (!File.Exists(ficheroConfiguracion))
            throw new FileNotFoundException(ficheroConfiguracion);

        builder
            .UseEnvironment(Environment)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
            })
            .ConfigureServices(ConfigureServices)
            // En medio se ejecutan los servicios de la app
            .ConfigureTestServices(ConfigureTestServices);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = true;

        //x.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = FakeJwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
        //}).AddFakeJwtBearer();

        //services.RemoveAll<IDateTime>().AddTransient(_ =>
        //{
        //    var dateTime = Substitute.For<IDateTime>();
        //    dateTime.UtcNow.Returns(DateTime ?? System.DateTime.UtcNow);
        //    return dateTime;
        //});
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
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

        //services.RemoveAll<IEmailSender>().AddTransient(_ => Substitute.For<IEmailSender>());

        //services.RemoveAll<IEventBus>().AddSingleton<IEventBus, SynchronousEventBus>();
    }

    public virtual async Task<HttpClient> CreateClientAsync(CancellationToken cancellationToken)
    {
        if (_firstTime)
        {
            await RegenerateDatabaseAsync(cancellationToken).ConfigureAwait(false);
            _firstTime = false;
        }

        if (_dataBase != default)
            await _dataBase.DisposeAsync().ConfigureAwait(false);

        _dataBase = new DatabaseManager(GetNewDbContext());

        var client = CreateClient();
        client.Timeout = TimeSpan.FromMinutes(20);
        client.DefaultRequestHeaders.Add("Accept-Language", Culture);
        return client;
    }

    protected virtual async Task RegenerateDatabaseAsync(CancellationToken cancellationToken)
    {
        var unitOfWork = GetNewDbContext();
        if (DeleteDatabase)
            await unitOfWork.Database.EnsureDeletedAsync(cancellationToken).ConfigureAwait(false);
        unitOfWork.Database.SetCommandTimeout(300);
        await unitOfWork.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
    }

    public DatabaseManager? Database() => _dataBase;

    public abstract DbContext GetNewDbContext();
}
