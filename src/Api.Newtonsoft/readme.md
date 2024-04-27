The following code demonstrates basic usage of SharedKernel api.

### appsettings.json

```json
{
  "AllowedHosts": "",
  "Origins": [
    "https://localhost/"
  ],
  "SmtpSettings": {
    "MailServer": "smtp",
    "MailPort": 587,
    "SenderName": "SharedKernel@SharedKernel.com",
    "Sender": "SharedKernel@SharedKernel.com",
    "Password": "SharedKernel"
  },
  "OpenApiOptions": {
    "Title": "SharedKernel",
    "AppName": "SharedKernel",
    "Name": "SharedKernel",
    "XmlDocumentationFile": "SharedKernel.Api.xml"
  },
  "ConnectionStrings": {
    "PaymentConnection": "Server=.;Database=Payment;Trusted_Connection=True;MultipleActiveResultSets=true;Application Name=Payment;",
    "PurchasingConnection": "Server=.;Database=Purchasing;Trusted_Connection=True;MultipleActiveResultSets=true;Application Name=Purchasing;"
  },
  "RabbitMq": {
    "Username": "guest",
    "Password": "guest",
    "Hostname": "localhost",
    "port": "5672"
  },
  "RedisCacheOptions": {
    "ConnectionString": "localhost:6379",
    "Configuration": "localhost:6379",
    "InstanceName": "sharedKernel"
  },
}
```

### Startup.cs

[See register module information](../Infrastructure/readme.md)

```cs
public class Startup
{
public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSharedKernel()
                .AddSharedKernelApi<FluentApiSampleValidator>(CorsPolicy, Configuration.GetSection("Origins").Get<string[]>())
                .AddSharedKernelHealthChecks()
                .AddSharedKernelOpenApi(Configuration)
                
                // Cache
                .AddRedisDistributedCache(Configuration)
                // .AddInMemoryCache()
                
                .AddInMemoryCommandBus()
                
                .AddInMemoryQueryBus()

                // Event bus
                .AddRabbitMqEventBus(Configuration)
                // .AddInMemoryEventBus()
                //.AddRedisEventBus(Configuration)

                // Add modules
                
                .AddPaymentModule(Configuration, "PaymentConnection")
                .AddPurchasingModule(Configuration, "PurchasingConnection")

                // Register all domain event subscribers
                .AddDomainEventSubscribers();
        }

        public void Configure(IApplicationBuilder app, IOptions<OpenApiOptions> options)
        {
            // Other usages

            app.UseCors(CorsPolicy);

            // Other usages 

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });

                    endpoints.MapControllers();
                })
                .UseSharedKernelMetrics()
                .UseSharedKernelOpenApi(options);
        }
    }
}
```