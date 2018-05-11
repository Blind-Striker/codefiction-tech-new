using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly HttpClient _client;

        public MeetupControllerTests()
        {
            _client = TestServerFactory.GetServer.CreateClient();
        }

        [Fact]
        public async Task Get_Meetups_Should_Return_MeetupModels_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/meetups");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<MeetupModel>>(responseStr);

            var ids = new[] {1, 2};

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Contains(response, model => ids.Contains(model.Id));
        }

        [Fact]
        public async Task Get_MeetupById_Should_Return_400_When_Given_Id_Is_Invalid()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/meetups/aaa");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_MeetupById_Should_Return_404_When_Given_Id_Is_NotFound()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/meetups/999");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_MeetupById_Should_Return_MeetupModel_By_Id_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/meetups/1");

            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<MeetupModel>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(1, response.Id);
            Assert.Equal("Birinci Meetup", response.Title);
            Assert.Contains(new[] {"Deniz İrgin", "Mert Susur"}, att => response.Attendees.Select(person => person.Name).Contains(att));
        }
    }
}
