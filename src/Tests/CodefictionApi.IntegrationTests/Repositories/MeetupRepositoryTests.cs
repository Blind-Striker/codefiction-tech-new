using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Repositories;
using Xunit;

namespace CodefictionApi.IntegrationTests.Repositories
{
    public class MeetupRepositoryTests
    {
        private readonly Database _database;

        public MeetupRepositoryTests()
        {
            _database = new Database()
            {
                Meetups = new[]
                {
                    new Meetup()
                    {
                        Id = 1,
                        Title = "Angular 2",
                        Date = DateTime.Now,
                        Description = "Bu bölümde Angular2 tartıştık",
                        MeetupLink = "link.com",
                        Attendees = new[] {"Deniz İrgin", "Deniz Özgen", "Barış Özaydın"},
                        VideoIds = new[] {1, 4, 5},
                        SponsorIds = new[] {1},
                        Photos = new[] {"image.jpg"}
                    },
                    new Meetup()
                    {
                        Id = 2,
                        Title = ".Net Core 2",
                        Date = DateTime.Now,
                        Description = "Bu bölümde .Net Core 2 tartıştık",
                        MeetupLink = "link.com",
                        Attendees = new[] {"Fırat Özbolat", "Mert Susur", "Mahmut Gündoğdu"},
                        VideoIds = new[] {1, 4, 5},
                        SponsorIds = new[] {2},
                        Photos = new[] {"image2.jpg"}
                    }
                }
            };
        }

        [Fact]
        public async Task GetMeetupById_Should_Get_Meetup_By_Given_Id()
        {            
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var meetupRepository = new MeetupRepository(memoryDatabaseProvider);

            var id = 2;

            Meetup meetup = await meetupRepository.GetMeetupById(id);
            Meetup dbMeetup = _database.Meetups.FirstOrDefault(m => m.Id == id);

            Assert.NotNull(meetup);
            Assert.NotNull(dbMeetup);
            Assert.Equal(meetup.Id, dbMeetup.Id);
            Assert.Equal(meetup.Title, dbMeetup.Title);
            Assert.Equal(meetup.Date, dbMeetup.Date);
            Assert.Equal(meetup.Description, dbMeetup.Description);
            Assert.Equal(meetup.MeetupLink, dbMeetup.MeetupLink);
            Assert.True(dbMeetup.Attendees != null && dbMeetup.Attendees.Any(s => meetup.Attendees.Contains(s)));
            Assert.True(dbMeetup.VideoIds != null && dbMeetup.VideoIds.Any(s => meetup.VideoIds.Contains(s)));
            Assert.True(dbMeetup.SponsorIds != null && dbMeetup.SponsorIds.Any(s => meetup.SponsorIds.Contains(s)));
            Assert.True(dbMeetup.Photos != null && dbMeetup.Photos.Any(s => meetup.Photos.Contains(s)));
        }

        [Fact]
        public async Task GetMeetups_Should_Get_All_Meetups()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var meetupRepository = new MeetupRepository(memoryDatabaseProvider);

            IList<Meetup> meetups = (await meetupRepository.GetMeetups()).ToList();

            foreach (Meetup dbMeetup in _database.Meetups.ToList())
            {
                Meetup meetup = meetups.FirstOrDefault(m => m.Id == dbMeetup.Id);

                Assert.NotNull(meetup);
                Assert.NotNull(dbMeetup);
                Assert.Equal(meetup.Id, dbMeetup.Id);
                Assert.Equal(meetup.Title, dbMeetup.Title);
                Assert.Equal(meetup.Date, dbMeetup.Date);
                Assert.Equal(meetup.Description, dbMeetup.Description);
                Assert.Equal(meetup.MeetupLink, dbMeetup.MeetupLink);
                Assert.True(dbMeetup.Attendees != null && dbMeetup.Attendees.Any(s => meetup.Attendees.Contains(s)));
                Assert.True(dbMeetup.VideoIds != null && dbMeetup.VideoIds.Any(s => meetup.VideoIds.Contains(s)));
                Assert.True(dbMeetup.SponsorIds != null && dbMeetup.SponsorIds.Any(s => meetup.SponsorIds.Contains(s)));
                Assert.True(dbMeetup.Photos != null && dbMeetup.Photos.Any(s => meetup.Photos.Contains(s)));
            }

            Assert.Equal(meetups.Count, _database.Meetups.Length);
            Assert.Contains(meetups, meetup => _database.Meetups.Select(m => m.Id).Contains(meetup.Id));
        }
    }
}
