using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;


//[assembly: OwinStartup(typeof(Startup))]
namespace NamedClient
{
    public class Startup
    {
        public static IServiceProvider Container { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {

            Container = RegisterServices();

            services.AddHangfire(configuration =>
            {
                configuration
                .UseColouredConsoleLogProvider()
                .UseMemoryStorage()
                .UseActivator(new ContainerJobActivator(Container));

            });
            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobManager)
        {

            app.UseHangfireDashboard("/hangfire");

            var service = Container.GetRequiredService<IEplatformRequestService>();
            RecurringJob.AddOrUpdate(() => service.Send(new HttpRequestMessage(HttpMethod.Head, "/hc")), Cron.Minutely);

        }

        private static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureHttpClients(serviceCollection);
            serviceCollection.AddTransient<EplatformRequestService>();
            serviceCollection.AddTransient<IEplatformRequestService, EplatformRequestService>();
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient("eplatform", client =>
            {
                client.BaseAddress = new Uri("https://health-status.eplatform.com.tr");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
