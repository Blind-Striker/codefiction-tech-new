using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace CodefictionApi.Core.Tests
{
    public class PodcastModelMapperTests
    {
        static PodcastModelMapperTests()
        {
            AutoMapperConfig.Init();
        }

        [Fact]
        public async Task Map_Should_Return_Null_If_Meetup_Is_Null()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            IPodcast podcast = null;

            IPodcastModel podcastModel = await mock.Map(podcast);

            Assert.Null(podcastModel);
        }

        [Fact]
        public async Task Map_List_Should_Return_Null_If_Meetup_Is_Null()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            IEnumerable<IPodcast> podcasts = null;

            IEnumerable<IPodcastModel> podcastModels = await mock.Map(podcasts);

            Assert.Null(podcastModels);
        }

        [Fact]
        public async Task Map_Should_Not_Set_IPodcastModel_Attendees_If_IPodcast_Attendees_Is_Null_Or_Empty()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            IPodcast podcast = new Podcast()
            {
                Id = 1,
                Season = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                SoundcloudId = "2313333",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] {"dotnet", "microsoft"},
                PublishDate = DateTime.Now,
                Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                Attendees = null
            };

            IPodcastModel podcastModel = await mock.Map(podcast);

            void AssertVerify()
            {
                mock.PersonService.Reset();

                mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
                Assert.Null(podcastModel.Attendees);
            }

            AssertVerify();

            podcast.Attendees = new string[0];
            podcastModel = await mock.Map(podcast);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_IPodcastModel_Attendees_If_IPodcast_Attendees_Is_Not_Null_Or_Empty()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            var attendeeName = "Deniz İrgin";

            IPodcast podcast = new Podcast()
            {
                Id = 1,
                Season = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                SoundcloudId = "2313333",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Attendees = new []{ attendeeName }
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => podcast.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> { new Person() { Id = 1, Name = attendeeName } });

            IPodcastModel podcastModel = await mock.Map(podcast);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Never);

            Assert.NotNull(podcastModel.Attendees);
            Assert.Equal(podcastModel.Attendees.Length, podcast.Attendees.Length);
            Assert.Equal(attendeeName, podcastModel.Attendees[0].Name);
        }

        [Fact]
        public async Task Map_Should_Not_Set_IPodcastModel_Guest_If_IPodcast_Guest_Is_Null_Or_Empty()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            IPodcast podcast = new Podcast()
            {
                Id = 1,
                Season = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                SoundcloudId = "2313333",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Guest = null
            };

            IPodcastModel podcastModel = await mock.Map(podcast);

            void AssertVerify()
            {
                mock.PersonService.Reset();

                mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Never);
                Assert.Null(podcastModel.Guest);
            }

            AssertVerify();

            podcast.Guest = string.Empty;
            podcastModel = await mock.Map(podcast);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_IPodcastModel_Guest_If_IPodcast_Guest_Is_Not_Null_Or_Empty()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            var guestName = "Deniz İrgin";

            IPodcast podcast = new Podcast()
            {
                Id = 1,
                Season = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                SoundcloudId = "2313333",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Guest = guestName
            };

            mock.PersonService
                .Setup(service => service.GetPersonByName(It.Is<string>(s => s == guestName)))
                .ReturnsAsync(() =>  new Person() { Id = 1, Name = guestName } );

            IPodcastModel podcastModel = await mock.Map(podcast);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Once);

            Assert.NotNull(podcastModel.Guest);
            Assert.Equal(guestName, podcastModel.Guest.Name);
        }

        [Fact]
        public async Task Map_Should_Set_IPodcastModel_Properties_Based_On_IPodcast_Properties()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            var guestName = "Fırat Özbolat";
            var attendeeName = "Deniz İrgin";

            IPodcast podcast = new Podcast()
            {
                Id = 1,
                Season = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                SoundcloudId = "2313333",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] {"dotnet", "microsoft"},
                PublishDate = DateTime.Now,
                Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                Attendees = new[] {attendeeName},
                Guest = guestName
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => podcast.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> { new Person() { Id = 1, Name = attendeeName } });

            mock.PersonService
                .Setup(service => service.GetPersonByName(It.Is<string>(s => s == guestName)))
                .ReturnsAsync(() => new Person() { Id = 1, Name = guestName });

            IPodcastModel podcastModel = await mock.Map(podcast);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);
            mock.PersonService.Verify(service => service.GetPersonByName(It.IsAny<string>()), Times.Once);

            Assert.Equal(podcast.Id, podcastModel.Id);
            Assert.Equal(podcast.Season, podcastModel.Season);
            Assert.Equal(podcast.Title, podcastModel.Title);
            Assert.Equal(podcast.Slug, podcastModel.Slug);
            Assert.Equal(podcast.YoutubeUrl, podcastModel.YoutubeUrl);
            Assert.Equal(podcast.ShortDescription, podcastModel.ShortDescription);
            Assert.Equal(podcast.LongDescription, podcastModel.LongDescription);
            Assert.Contains(podcast.Tags, s => podcastModel.Tags.Contains(s));
            Assert.Equal(podcast.PublishDate, podcastModel.PublishDate);
            Assert.Equal(podcast.Relations.Length, podcastModel.Relations.Length);
            Assert.NotNull(podcastModel.Attendees);
            Assert.Equal(podcastModel.Attendees.Length, podcast.Attendees.Length);
            Assert.Equal(attendeeName, podcastModel.Attendees[0].Name);
            Assert.NotNull(podcastModel.Guest);
            Assert.Equal(guestName, podcastModel.Guest.Name);
        }

        [Fact]
        public async Task Map_List_Should_Return_Equal_Number_Of_Given_Podcast()
        {
            PodcastModelMapperMock mock = PodcastModelMapperMock.Create();

            IList<Podcast> podcasts = new List<Podcast>()
            {
                new Podcast()
                {
                    Id = 1,
                    Season = 1,
                    Title = "Dotnet Core",
                    Slug = "dotnet-core",
                    YoutubeUrl = "youtube.com",
                    SoundcloudId = "2313333",
                    ShortDescription = "Çok Kısa",
                    LongDescription = "Long",
                    Tags = new[] {"dotnet", "microsoft"},
                    PublishDate = DateTime.Now,
                    Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                },
                new Podcast()
                {
                    Id = 2,
                    Season = 1,
                    Title = "Angular iki",
                    Slug = "angualr-iki",
                    YoutubeUrl = "youtube.com",
                    SoundcloudId = "2313333",
                    ShortDescription = "Çok Kısa",
                    LongDescription = "Long",
                    Tags = new[] {"dotnet", "microsoft"},
                    PublishDate = DateTime.Now,
                    Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                }
            };

            IEnumerable<IPodcastModel> podcastModels = await mock.Map(podcasts);

            Assert.NotNull(podcastModels);
            Assert.Equal(podcasts.Count, podcastModels.Count());
        }
    }
}
