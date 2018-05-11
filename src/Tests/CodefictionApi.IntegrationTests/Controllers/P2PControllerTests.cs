using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Codefiction.CodefictionTech.CodefictionApi.Server;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CodefictionApi.IntegrationTests.Controllers
{
    public class P2PControllerTests
    {
        private readonly HttpClient _client;

        public P2PControllerTests()
        {
            _client = TestServerFactory.GetServer.CreateClient();
        }

        [Fact]
        public async Task Get_P2Ps_Should_Return_P2PModels_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/P2Ps");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<P2PModel>>(responseStr);

            var ids = new[] { 1, 2 };

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Contains(response, model => ids.Contains(model.Id));
        }

        [Fact]
        public async Task Get_P2PBySlug_Should_Return_404_When_Given_Id_Is_NotFound()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/P2Ps/sadasdasdsadas");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_P2PBySlug_Should_Return_P2PModel_By_Id_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/P2Ps/birinci-bolum-yazilim-mimarligi-ve-docker-ibrahim-gunduz");

            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<P2PModel>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(1, response.Id);
            Assert.Equal("Yazılım Mimarlığı ve Docker", response.Title);
            Assert.Contains(new[] {"Barış Özaydın", "Deniz İrgin", "Fırat Özbolat", "Uğur Atar", "Onur Aykaç"},
                            att => response.Attendees.Select(person => person.Name).Contains(att));
        }
    }
}
