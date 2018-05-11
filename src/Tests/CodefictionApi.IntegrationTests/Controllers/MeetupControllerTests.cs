using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Codefiction.CodefictionTech.CodefictionApi.Server;
using CodefictionApi.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CodefictionApi.IntegrationTests.Controllers
{
    public class MeetupControllerTests
    {
        private static readonly TestServer Server;

        private readonly HttpClient _client;

        static MeetupControllerTests()
        {
            IWebHostBuilder webHostBuilder = new WebHostBuilder().UseStartup<Startup>();

            Server = new TestServer(webHostBuilder);
        }

        public MeetupControllerTests()
        {
            _client = Server.CreateClient();
        }

        [Fact]
        public async Task Get_Meetups_Should_Return_MeetupModel_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/meetups");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<MeetupModel>>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
    }
}
