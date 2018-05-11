using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Models;
using CodefictionApi.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace CodefictionApi.Core.Tests
{
    public class MeetupModelMapperTests
    {
        static MeetupModelMapperTests()
        {
            AutoMapperConfig.Init();
        }

        [Fact]
        public async Task Map_Should_Return_Null_If_Meetup_Is_Null()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            Meetup meetup = null;

            MeetupModel meetupModel = await mock.Map(meetup);

            Assert.Null(meetupModel);
        }

        [Fact]
        public async Task Map_Should_Not_Set_MeetupModel_Attendees_If_Meetup_Attendees_Is_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                Attendees = null,
                Photos = new[] {""}
            };

            MeetupModel meetupModel = await mock.Map(meetup);

            void AssertVerify()
            {
                mock.PersonService.Reset();

                mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
                Assert.Null(meetupModel.Attendees);
            }

            AssertVerify();

            meetup.Attendees = new string[0];
            meetupModel = await mock.Map(meetup);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_MeetupModel_Attendees_If_Meetup_Attendees_Is_Not_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var attendeeName = "Deniz İrgin";

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                Attendees = new[] { attendeeName }
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => meetup.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> {new Person() {Id = 1, Name = attendeeName } });

            MeetupModel meetupModel = await mock.Map(meetup);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Never);
            mock.SponsorService.Verify(service => service.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Never);

            Assert.NotNull(meetupModel.Attendees);
            Assert.Equal(meetupModel.Attendees.Length, meetup.Attendees.Length);
            Assert.Equal(attendeeName, meetupModel.Attendees[0].Name);
        }

        [Fact]
        public async Task Map_Should_Not_Set_MeetupModel_Videos_If_Meetup_VideoIds_Is_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                VideoIds = null
            };

            MeetupModel meetupModel = await mock.Map(meetup);

            void AssertVerify()
            {
                mock.VideoService.Reset();

                mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Never);
                Assert.Null(meetupModel.Videos);
            }

            AssertVerify();

            meetup.VideoIds = new int[0];
            meetupModel = await mock.Map(meetup);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_MeetupModel_Videos_If_Meetup_VideoIds_Is_Not_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var videoTitle = "Live Coding";

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                VideoIds = new[] { 1 }
            };

            mock.VideoService
                .Setup(service => service.GetVideosByIds(It.Is<IList<int>>(list => list.All(s => meetup.VideoIds.Contains(s)))))
                .ReturnsAsync(() => new List<VideoModel> { new VideoModel() { Id = 1, Title = videoTitle } });

            MeetupModel meetupModel = await mock.Map(meetup);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Once);
            mock.SponsorService.Verify(service => service.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Never);

            Assert.NotNull(meetupModel.Videos);
            Assert.Equal(meetupModel.Videos.Length, meetup.VideoIds.Length);
            Assert.Equal(videoTitle, meetupModel.Videos[0].Title);
        }

        [Fact]
        public async Task Map_Should_Not_Set_MeetupModel_Sponsors_If_Meetup_SponsorIds_Is_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                SponsorIds = null
            };

            MeetupModel meetupModel = await mock.Map(meetup);

            void AssertVerify()
            {
                mock.SponsorService.Reset();

                mock.SponsorService.Verify(service => service.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Never);
                Assert.Null(meetupModel.Sponsors);
            }

            AssertVerify();

            meetup.SponsorIds = new int[0];
            meetupModel = await mock.Map(meetup);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_MeetupModel_Sponsors_If_Meetup_SponsorIds_Is_Not_Null_Or_Empty()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var sponsorName = "armut.com";

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                SponsorIds = new[] { 1 }
            };

            mock.SponsorService
                .Setup(service => service.GetSponsorsByIds(It.Is<IList<int>>(list => list.All(s => meetup.SponsorIds.Contains(s)))))
                .ReturnsAsync(() => new List<Sponsor> { new Sponsor { Id = 1, Name = sponsorName} });

            MeetupModel meetupModel = await mock.Map(meetup);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Never);
            mock.SponsorService.Verify(service => service.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Once);

            Assert.NotNull(meetupModel.Sponsors);
            Assert.Equal(meetupModel.Sponsors.Length, meetup.SponsorIds.Length);
            Assert.Equal(sponsorName, meetupModel.Sponsors[0].Name);
        }

        [Fact]
        public async Task Map_Should_Set_MeetupModel_Properties_Based_On_Meetup_Properties()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            var sponsorName = "armut.com";
            var attendeeName = "Deniz İrgin";
            var videoTitle = "Live Coding";

            var meetup = new Meetup()
            {
                Id = 1,
                Title = "Birinci",
                Description = "Hede",
                MeetupLink = "sample.com",
                Date = DateTime.Now,
                SponsorIds = new[] { 1 },
                Attendees = new []{attendeeName},
                VideoIds = new []{1},
                Photos = new[] {"sample.jpg"}
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => meetup.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> { new Person() { Id = 1, Name = attendeeName } });

            mock.VideoService
                .Setup(service => service.GetVideosByIds(It.Is<IList<int>>(list => list.All(s => meetup.VideoIds.Contains(s)))))
                .ReturnsAsync(() => new List<VideoModel> { new VideoModel() { Id = 1, Title = videoTitle } });

            mock.SponsorService
                .Setup(service => service.GetSponsorsByIds(It.Is<IList<int>>(list => list.All(s => meetup.SponsorIds.Contains(s)))))
                .ReturnsAsync(() => new List<Sponsor> { new Sponsor { Id = 1, Name = sponsorName } });

            MeetupModel meetupModel = await mock.Map(meetup);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Once);
            mock.SponsorService.Verify(service => service.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Once);

            Assert.Equal(meetup.Id, meetupModel.Id);
            Assert.Equal(meetup.Title, meetupModel.Title);
            Assert.Equal(meetup.Description, meetupModel.Description);
            Assert.Equal(meetup.MeetupLink, meetupModel.MeetupLink);
            Assert.Equal(meetup.Date, meetupModel.Date);
            Assert.NotNull(meetupModel.Attendees);
            Assert.Equal(meetupModel.Attendees.Length, meetup.Attendees.Length);
            Assert.Equal(attendeeName, meetupModel.Attendees[0].Name);
            Assert.NotNull(meetupModel.Videos);
            Assert.Equal(meetupModel.Videos.Length, meetup.VideoIds.Length);
            Assert.Equal(videoTitle, meetupModel.Videos[0].Title);
            Assert.NotNull(meetupModel.Sponsors);
            Assert.Equal(meetupModel.Sponsors.Length, meetup.SponsorIds.Length);
            Assert.Equal(sponsorName, meetupModel.Sponsors[0].Name);
        }

        [Fact]
        public async Task Map_List_Should_Return_Null_If_Meetup_Is_Null()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            IList<Meetup> meetup = null;

            IEnumerable<MeetupModel> meetupModels = await mock.Map(meetup);

            Assert.Null(meetupModels);
        }

        [Fact]
        public async Task Map_List_Should_Return_Equal_Number_Of_Given_Meetup()
        {
            MeetupModelMapperMock mock = MeetupModelMapperMock.Create();

            IList<Meetup> meetup = new List<Meetup>()
            {
                new Meetup()
                {
                    Id = 1,
                    Title = "Birinci",
                    Description = "Hede",
                    MeetupLink = "sample.com",
                    Date = DateTime.Now,
                    Photos = new[] {"sample.jpg"}
                },
                new Meetup()
                {
                    Id = 2,
                    Title = "İkinci",
                    Description = "Hödö",
                    MeetupLink = "sample.com",
                    Date = DateTime.Now,
                    Photos = new[] {"sample.jpg"}
                }
            };

            IEnumerable<MeetupModel> meetupModels = await mock.Map(meetup);

            Assert.NotNull(meetupModels);
            Assert.Equal(meetup.Count, meetupModels.Count());
        }
    }
}
