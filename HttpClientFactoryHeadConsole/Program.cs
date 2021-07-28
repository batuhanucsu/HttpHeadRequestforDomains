using System;
using System.Net.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NamedClient
{
    class Program
    {

        static void Main()
        {
            // Configure services
            
            //var services = new ServiceCollection();

            //GlobalConfiguration.Configuration
            //                            .UseColouredConsoleLogProvider()
            //                            .UseMemoryStorage()
            //                            .UseActivator(new ContainerJobActivator(Container));

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls("http://localhost:5000")
            .UseStartup<Startup>()
            .Build();
            host.Run();

            
            //ePlatformRequestService.Send(new HttpRequestMessage(HttpMethod.Head, "/hc"));
            // starting a new server.
            //using (var server = new BackgroundJobServer())
            //{
            //    Console.WriteLine("starting hangfire job server");
            //    Console.ReadKey();
            //}

        }


    }
}
