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


            GlobalConfiguration.Configuration
                                        .UseColouredConsoleLogProvider()
                                        .UseMemoryStorage();
           

            RecurringJob.AddOrUpdate(() => new Client().HeadEplatform(), Cron.Minutely);
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
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient("eplatform", client =>
            {
                client.BaseAddress = new Uri("https://health-status.eplatform.com.tr/hc");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                
            });


        }
    }
}
