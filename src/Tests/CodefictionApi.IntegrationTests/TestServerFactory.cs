using Codefiction.CodefictionTech.CodefictionApi.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace CodefictionApi.IntegrationTests
{
    public static class TestServerFactory
    {
        static TestServerFactory()
        {
            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                                             .UseSetting("application", "test")
                                             .UseSetting("DatabaseJson", "./Databases/test-db.json")
                                             .UseStartup<Startup>();

            GetServer = new TestServer(webHostBuilder);
        }

        public static TestServer GetServer { get; }
    }
}
