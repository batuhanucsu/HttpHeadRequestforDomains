using Hangfire;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NamedClient
{
    public class ContainerJobActivator : JobActivator
    {
        private IServiceProvider _container;

        public ContainerJobActivator(IServiceProvider serviceProvider)
        {
            _container = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            var a= _container.GetService(type);
            return a;
        }
    }

    public interface IEplatformRequestService
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage httpRequestMessage);
    }

    public class EplatformRequestService : IEplatformRequestService
    {
        private readonly IHttpClientFactory _clientFactory;

        public EplatformRequestService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> Send(HttpRequestMessage httpRequestMessage)
        {
            var httpClient = _clientFactory.CreateClient("eplatform");
            var response = await httpClient.SendAsync(httpRequestMessage);//.ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            //response.ToString();
            Console.WriteLine(response.ToString());
            //return await response.Content.ReadAsStringAsync();

            return response;
        }
    }
}
