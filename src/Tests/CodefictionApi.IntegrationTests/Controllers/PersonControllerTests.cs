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
    public class PersonControllerTests
    {
        private readonly HttpClient _client;

        public PersonControllerTests()
        {
            _client = TestServerFactory.GetServer.CreateClient();
        }

        [Fact]
        public async Task Get_PeopleByType_Should_Return_Crew_And_200_As_Response_If_Type_Is_Crew()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/people/type/crew");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<Person>>(responseStr);

            var ids = new[] { 1, 2, 4, 5, 6, 7 };

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Contains(response, model => ids.Contains(model.Id));
        }

        [Fact]
        public async Task Get_PeopleByType_Should_Return_Guests_And_200_As_Response_If_Type_Is_Crew()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/people/type/guest");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<Person>>(responseStr);

            var ids = new[] { 12, 13, 14 };

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Contains(response, model => ids.Contains(model.Id));
        }

        [Fact]
        public async Task Get_PeopleByType_Should_Return_Empty_List_And_200_As_Response_If_Type_Is_Unknown()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/people/type/sdsadsa");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<IEnumerable<Person>>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Empty(response);
        }

        [Fact]
        public async Task Get_PersonByName_Should_Return_404_When_Given_Id_Is_NotFound()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/people/name/İzzet Şapkalıoğlu");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Get_PersonByName_Should_Return_Person_By_Id_And_200_As_Response()
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/people/name/Barış Özaydın");

            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Person>(responseStr);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(1, response.Id);
            Assert.Equal("Barış Özaydın", response.Name);
            Assert.Equal("Crew", response.Type);
            Assert.Equal("ozaydin.baris@gmail.com", response.Email);
        }

        [Fact]
        public async Task Get_PeopleByNames_Should_Return_People_And_200()
        {
            var people = new[] {"Uğur Atar", "Başar Köprücü"};

            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"api/people?names={people[0]}&names={people[1]}");
            string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();

            IList<Person> response = JsonConvert.DeserializeObject<IEnumerable<Person>>(responseStr).ToList();

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(people.Length, response.Count());
            Assert.Contains(response, person => people.Contains(person.Name));
        }
    }
}
