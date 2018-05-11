using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CodefictionApi.Tests
{
    public class PersonControllerTests
    {
        [Fact]
        public async Task PeopleByType_Should_Call_IPersonService_GetCrew_And_Return_Ok_If_Type_Is_Crew()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            mock.PersonService.Setup(service => service.GetCrew()).ReturnsAsync(() => new List<Person>());

            IActionResult actionResult = await mock.PeopleByType("crew");

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var people = okObjectResult.Value as List<Person>;

            Assert.NotNull(people);
            mock.PersonService.Verify(service => service.GetCrew(), Times.Once);
            mock.PersonService.Verify(service => service.GetGuests(), Times.Never);
        }

        [Fact]
        public async Task PeopleByType_Should_Call_IPersonService_GetGuests_And_Return_Ok_If_Type_Is_Guest()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            mock.PersonService.Setup(service => service.GetGuests()).ReturnsAsync(() => new List<Person>());

            IActionResult actionResult = await mock.PeopleByType("guest");

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var people = okObjectResult.Value as List<Person>;

            Assert.NotNull(people);
            mock.PersonService.Verify(service => service.GetCrew(), Times.Never);
            mock.PersonService.Verify(service => service.GetGuests(), Times.Once);
        }

        [Fact]
        public async Task PeopleByType_Should_Return_Empty_And_Ok_If_Type_Is_Unknown()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            mock.PersonService.Setup(service => service.GetCrew()).ReturnsAsync(() => new List<Person>());

            IActionResult actionResult = await mock.PeopleByType("asdasge");

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var people = okObjectResult.Value as List<Person>;

            Assert.NotNull(people);
            Assert.Empty(people);
            mock.PersonService.Verify(service => service.GetCrew(), Times.Never);
            mock.PersonService.Verify(service => service.GetGuests(), Times.Never);
        }

        [Fact]
        public async Task PersonByName_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            PersonControllerMock mock = PersonControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.PersonByName("Deniz Özgen");

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PersonByName_Should_Call_IPersonService_GetPersonByName_And_Return_NotFound_If_IPersonService_GetPersonByName_Returns_Null()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            mock.PersonService.Setup(service => service.GetPersonByName(It.IsAny<string>())).ReturnsAsync(() => null);

            IActionResult actionResult = await mock.PersonByName("Fırat Özbolat");

            var notFoundResult = actionResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PersonByName_Should_Call_IPersonService_GetPersonByName_And_Return_Ok()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            var person = "Ahmet Erdem Kahveci";

            mock.PersonService.Setup(service => service.GetPersonByName(It.Is<string>(i => i == person))).ReturnsAsync(() => new Person());

            IActionResult actionResult = await mock.PersonByName(person);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var personModel = okObjectResult.Value as Person;

            Assert.NotNull(personModel);
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PeopleByNames_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            PersonControllerMock mock = PersonControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.PeopleByNames(new List<string>() {"Deniz Özgen", "Mahmut Gündoğdu"});

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
        }

        [Fact]
        public async Task PeopleByNames_Should_Call_IPersonService_GetPeopleByNames_And_Return_Ok()
        {
            PersonControllerMock mock = PersonControllerMock.Create();

            var people = new List<string>() { "Uğur Aldanmaz", "Özgün Bal" };

            mock.PersonService.Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(p => p.All(ps => people.Contains(ps))))).ReturnsAsync(() => new List<Person>());

            IActionResult actionResult = await mock.PeopleByNames(people);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var peopleModels = okObjectResult.Value as IEnumerable<Person>;

            Assert.NotNull(peopleModels);
            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
        }
    }
}
