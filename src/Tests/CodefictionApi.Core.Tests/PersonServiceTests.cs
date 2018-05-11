using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace CodefictionApi.Core.Tests
{
    public class PersonServiceTests
    {
        [Fact]
        public async Task GetPersonById_Should_Call_IPersonRepository_GetPersonById()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            var id = 1;

            mock.PersonRepository
                .Setup(repository => repository.GetPersonById(It.Is<int>(i => i == id)))
                .ReturnsAsync(() => new Person() {Id = id, Name = "Barış Özaydın"});

            Person person = await mock.GetPersonById(id);

            mock.PersonRepository.Verify(repository => repository.GetPersonById(It.IsAny<int>()), Times.Once);
            Assert.NotNull(person);
        }

        [Fact]
        public async Task GetPersonByName_Should_Throw_ArgumentNullException_If_Name_Is_Null()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            string name = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => mock.GetPersonByName(name));
            mock.PersonRepository.Verify(repository => repository.GetPersonByName(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetPersonByName_Should_Call_IPersonRepository_GetPersonByName()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            var id = 1;
            var name = "Barış Özaydın";

            mock.PersonRepository
                .Setup(repository => repository.GetPersonByName(It.Is<string>(i => i == name)))
                .ReturnsAsync(() => new Person() { Id = id, Name = name });

            Person person = await mock.GetPersonByName(name);

            mock.PersonRepository.Verify(repository => repository.GetPersonByName(It.IsAny<string>()), Times.Once);
            Assert.NotNull(person);
        }

        [Fact]
        public async Task GetCrew_Should_Call_IPersonRepository_GetCrew()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            var personList = new List<Person>() {new Person() {Id = 1, Name = "Mert Susur"}};

            mock.PersonRepository
                .Setup(repository => repository.GetCrew())
                .ReturnsAsync(() => personList);

            IEnumerable<Person> persons = await mock.GetCrew();

            mock.PersonRepository.Verify(repository => repository.GetCrew(), Times.Once);
            Assert.NotNull(persons);
            Assert.Equal(personList.Count, persons.Count());
            Assert.Equal(personList[0].Name, personList[0].Name);
        }

        [Fact]
        public async Task GetGuests_Should_Call_IPersonRepository_GetGuests()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            var personList = new List<Person>() { new Person() { Id = 1, Name = "Başar Köprücü" } };

            mock.PersonRepository
                .Setup(repository => repository.GetGuests())
                .ReturnsAsync(() => personList);

            IEnumerable<Person> persons = await mock.GetGuests();

            mock.PersonRepository.Verify(repository => repository.GetGuests(), Times.Once);
            Assert.NotNull(persons);
            Assert.Equal(personList.Count, persons.Count());
            Assert.Equal(personList[0].Name, personList[0].Name);
        }

        [Fact]
        public async Task GetPeopleByNames_Should_Throw_ArgumentNullException_If_Name_Is_Null()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            IList<string> names = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => mock.GetPeopleByNames(names));
            mock.PersonRepository.Verify(repository => repository.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
        }

        [Fact]
        public async Task GetPeopleByNames_Should_Call_IPersonRepository_GetPeopleByNames()
        {
            PersonServiceMock mock = PersonServiceMock.Create();

            IList<string> peopleList = new List<string>() {"Uğur Atar", "Onur Aykaç", "Deniz Özgen"};

            mock.PersonRepository
                .Setup(repository => repository.GetPeopleByNames(It.Is<IList<string>>(list => list.All(p => peopleList.Contains(p)))))
                .ReturnsAsync(() => new List<Person>()
                {
                    new Person() {Id = 1, Name = "Uğur Atar"},
                    new Person() {Id = 2, Name = "Onur Aykaç"},
                    new Person() {Id = 3, Name = "Deniz Özgen"}
                });

            IEnumerable<Person> people = await mock.GetPeopleByNames(peopleList);

            mock.PersonRepository.Verify(repository => repository.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
            Assert.NotNull(people);
            Assert.Equal(people.Count(), peopleList.Count);
        }
    }
}
