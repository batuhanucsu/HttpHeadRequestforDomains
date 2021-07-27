using System;
using System.Net.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace NamedClient
{
    class Program
    {
        public static IServiceProvider Container { get; private set; }

        static void Main()
        {
            // Configure services
            Container = RegisterServices();
            //var services = new ServiceCollection();

            GlobalConfiguration.Configuration
                                        .UseColouredConsoleLogProvider()
                                        .UseMemoryStorage()
                                        //;
                                        .UseActivator(new ContainerJobActivator(Container));

            var service = Container.GetRequiredService<IEplatformRequestService>();
            RecurringJob.AddOrUpdate(() => service.Send(new HttpRequestMessage(HttpMethod.Head, "/hc")), Cron.Minutely);
            //ePlatformRequestService.Send(new HttpRequestMessage(HttpMethod.Head, "/hc"));
            // starting a new server.
            using (var server = new BackgroundJobServer())
            {
                Console.WriteLine("starting hangfire job server");
                Console.ReadKey();
            }

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
