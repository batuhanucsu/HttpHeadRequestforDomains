using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace NamedClient
{
    class Client
    {
        private readonly NamedRequestor _requestor;

        public Client()
        {
            var _httpClientFactory = Program.Container.GetRequiredService<IHttpClientFactory>();
            _requestor = new NamedRequestor(_httpClientFactory);
        }

        public async Task<string> HeadEplatform()
        {
            var headInfo = await _requestor.EplatformHeadReq();
            return headInfo;
        }

    }
}
