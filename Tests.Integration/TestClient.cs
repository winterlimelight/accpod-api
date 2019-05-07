using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Tests.Integration
{
    public class TestClient
    {
        public HttpClient Client { get; private set; }

        public TestClient()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Api.Startup>());

            Client = server.CreateClient();
        }
    }
}
