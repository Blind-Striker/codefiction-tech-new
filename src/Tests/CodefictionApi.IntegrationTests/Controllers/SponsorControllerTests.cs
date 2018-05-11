using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using Newtonsoft.Json;
using Xunit;

namespace CodefictionApi.IntegrationTests.Controllers
{
    public class SponsorControllerTests
    {
        private readonly HttpClient _client;

        public SponsorControllerTests()
        {
            _client = TestServerFactory.GetServer.CreateClient();
        }

        [Fact]
        public async Task Get_Sponsors_Should_Return_Sponsors_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/sponsors");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<Sponsor>>(responseStr);

            var ids = new[] { 1, 2, 3 };

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Contains(response, model => ids.Contains(model.Id));
        }

        [Fact]
        public async Task Get_SponsorById_Should_Return_400_When_Given_Id_Is_Invalid()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/sponsors/aaa");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_SponsorById_Should_Return_404_When_Given_Id_Is_NotFound()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/sponsors/999");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_SponsorById_Should_Return_Sponsor_By_Id_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/sponsors/1");

            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Sponsor>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(1, response.Id);
            Assert.Equal("armut.com", response.Name);
            Assert.Equal("armut.png", response.LogoUrl);
        }
    }
}
