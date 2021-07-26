using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NamedClient
{
    public class NamedRequestor
    {
        private readonly IHttpClientFactory _clientFactory;

        public NamedRequestor(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> EplatformHeadReq()
        {
            var request = new HttpRequestMessage(HttpMethod.Head, "https://health-status.eplatform.com.tr/hc");
            var httpClient = _clientFactory.CreateClient("eplatform");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            response.ToString();
            Console.WriteLine(response);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
