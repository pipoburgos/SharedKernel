//using System;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;

//namespace SharedKernel.Application.Tests.Shared
//{
//    public abstract class ApplicationTestCase<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
//    {
//        protected override void ConfigureWebHost(IWebHostBuilder builder)
//        {
//            builder?.ConfigureServices(Services());
//        }

//        protected new abstract Action<IServiceCollection> Services();
//    }
//}