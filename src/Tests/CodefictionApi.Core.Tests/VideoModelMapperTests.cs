using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Models;
using CodefictionApi.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace CodefictionApi.Core.Tests
{
    public class VideoModelMapperTests
    {
        static VideoModelMapperTests()
        {
            AutoMapperConfig.Init();
        }

        [Fact]
        public async Task Map_Should_Return_Null_If_Meetup_Is_Null()
        {
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            IVideo video = null;

            VideoModel videoModel = await mock.Map(video);

            Assert.Null(videoModel);
        }

        [Fact]
        public async Task Map_List_Should_Return_Null_If_Meetups_Is_Null()
        {
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            IEnumerable<IVideo> videos = null;

            IEnumerable<VideoModel> videoModels = await mock.Map(videos);

            Assert.Null(videoModels);
        }

        [Fact]
        public async Task Map_Should_Not_Set_VideoModel_Attendees_If_IVideo_Attendees_Is_Null_Or_Empty()
        {
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            IVideo video = new Video()
            {
                Id = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Type = "Meetup",
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Attendees = null
            };

            VideoModel videoModel = await mock.Map(video);

            void AssertVerify()
            {
                mock.PersonService.Reset();

                mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Never);
                Assert.Null(videoModel.Attendees);
            }

            AssertVerify();

            video.Attendees = new string[0];
            videoModel = await mock.Map(video);

            AssertVerify();
        }

        [Fact]
        public async Task Map_Should_Set_VideoModel_Attendees_If_IVideo_Attendees_Is_Not_Null_Or_Empty()
        {         
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            var attendeeName = "Deniz İrgin";

            IVideo video = new Video()
            {
                Id = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Type = "Meetup",
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Attendees = new []{ attendeeName }
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => video.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> { new Person() { Id = 1, Name = attendeeName } });

            VideoModel videoModel = await mock.Map(video);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);

            Assert.NotNull(videoModel.Attendees);
            Assert.Equal(videoModel.Attendees.Length, video.Attendees.Length);
            Assert.Equal(attendeeName, videoModel.Attendees[0].Name);
        }

        [Fact]
        public async Task Map_Should_Set_VideoModel_Properties_Based_On_IVideo_Properties()
        {
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            var attendeeName = "Deniz İrgin";

            IVideo video = new Video()
            {
                Id = 1,
                Title = "Dotnet Core",
                Slug = "dotnet-core",
                YoutubeUrl = "youtube.com",
                ShortDescription = "Çok Kısa",
                LongDescription = "Long",
                Tags = new[] { "dotnet", "microsoft" },
                PublishDate = DateTime.Now,
                Type = "Meetup",
                Relations = new[] { new Relation() { Id = 1, Type = "medium" } },
                Attendees = new[] { attendeeName }
            };

            mock.PersonService
                .Setup(service => service.GetPeopleByNames(It.Is<IList<string>>(list => list.All(s => video.Attendees.Contains(s)))))
                .ReturnsAsync(() => new List<Person> { new Person() { Id = 1, Name = attendeeName } });

            VideoModel videoModel = await mock.Map(video);

            mock.PersonService.Verify(service => service.GetPeopleByNames(It.IsAny<IList<string>>()), Times.Once);

            Assert.Equal(video.Id, videoModel.Id);
            Assert.Equal(video.Type, videoModel.Type);
            Assert.Equal(video.Title, videoModel.Title);
            Assert.Equal(video.Slug, videoModel.Slug);
            Assert.Equal(video.YoutubeUrl, videoModel.YoutubeUrl);
            Assert.Equal(video.ShortDescription, videoModel.ShortDescription);
            Assert.Equal(video.LongDescription, videoModel.LongDescription);
            Assert.Contains(video.Tags, s => videoModel.Tags.Contains(s));
            Assert.Equal(video.PublishDate, videoModel.PublishDate);
            Assert.Equal(video.Relations.Length, videoModel.Relations.Length);
            Assert.NotNull(videoModel.Attendees);
            Assert.Equal(videoModel.Attendees.Length, video.Attendees.Length);
            Assert.Equal(attendeeName, videoModel.Attendees[0].Name);
        }

        [Fact]
        public async Task Map_List_Should_Return_Equal_Number_Of_Given_Podcast()
        {
            VideoModelMapperMock mock = VideoModelMapperMock.Create();

            IList<IVideo> videos = new List<IVideo>()
            {
                new Video()
                {
                    Id = 1,
                    Title = "Dotnet Core",
                    Slug = "dotnet-core",
                    YoutubeUrl = "youtube.com",
                    ShortDescription = "Çok Kısa",
                    LongDescription = "Long",
                    Type = "Meetup",
                    Tags = new[] {"dotnet", "microsoft"},
                    PublishDate = DateTime.Now,
                    Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                },
                new Video()
                {
                    Id = 2,
                    Title = "Angular iki",
                    Slug = "angualr-iki",
                    YoutubeUrl = "youtube.com",
                    ShortDescription = "Çok Kısa",
                    LongDescription = "Long",
                    Type = "Meetup",
                    Tags = new[] {"dotnet", "microsoft"},
                    PublishDate = DateTime.Now,
                    Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                }
            };

            IEnumerable<VideoModel> videoModels = await mock.Map(videos);

            Assert.NotNull(videoModels);
            Assert.Equal(videos.Count, videoModels.Count());
        }
    }
}
