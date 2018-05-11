using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Repositories;
using Xunit;

namespace CodefictionApi.IntegrationTests.Repositories
{
    public class VideoRepositoryTests
    {
        private readonly Database _database;

        public VideoRepositoryTests()
        {
            _database = new Database()
            {
                Videos = new[]
                {
                    new Video()
                    {
                        Id = 1,
                        Title = "Ethereum Metropolis Sürümü",
                        Slug = "ethereum-metropolis-surumu",
                        ShortDescription = "Bu video'da Solidity dilini kullanarak akilli kontrat nasil hazirlanir anlatmaya calistik. ",
                        LongDescription = "",
                        Attendees = new[] {"Mert Susur"},
                        Type = "Training",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-Xqax3MOuPg",
                        Relations = new[] {new Relation() {Id = 1, Type = "Ethereum"}},
                        Tags = new[] {"ethereum"},
                        PublishDate = DateTime.Now
                    },
                    new Video()
                    {
                        Id = 2,
                        Title = "Ethereum 101",
                        Slug = "ethereum-101",
                        ShortDescription ="Ethereum 101",
                        LongDescription = "",
                        Attendees = new[] {"Mert Susur"},
                        Type = "Training",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-sssOuPg",
                        Relations = new[] {new Relation() {Id = 1, Type = "Ethereum"}},
                        Tags = new[] {"ethereum"},
                        PublishDate = DateTime.Now
                    },
                    new Video()
                    {
                        Id = 3,
                        Title = "Akka.Net Live Coding",
                        Slug = "akka-dotnet-live-coding",
                        ShortDescription ="Akka Live Coding",
                        LongDescription = "",
                        Attendees = new[] {"Deniz", "Mert Susur"},
                        Type = "Live Coding",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-XqfffOuPg",
                        Relations = new[] {new Relation() {Id = 1, Type = "Akka.Net"}},
                        Tags = new[] {"akka.net"},
                        PublishDate = DateTime.Now
                    }
                }
            };
        }

        [Fact]
        public async Task GetVideoById_Should_Get_Video_By_Given_Id()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var videoRepository = new VideoRepository(memoryDatabaseProvider);

            var id = 1;

            Video video = await videoRepository.GetVideoById(id);
            Video dbVideo = _database.Videos.FirstOrDefault(m => m.Id == id);

            Assert.NotNull(video);
            Assert.NotNull(dbVideo);
            Assert.Equal(video.Id, dbVideo.Id);
            Assert.Equal(video.Title, dbVideo.Title);
            Assert.Equal(video.Slug, dbVideo.Slug);
            Assert.Equal(video.ShortDescription, dbVideo.ShortDescription);
            Assert.Equal(video.LongDescription, dbVideo.LongDescription);
            Assert.Equal(video.YoutubeUrl, dbVideo.YoutubeUrl);
            Assert.Equal(video.Title, dbVideo.Title);
            Assert.Equal(video.Type, dbVideo.Type);
            Assert.Equal(video.PublishDate, dbVideo.PublishDate);
            Assert.True(dbVideo.Attendees != null && dbVideo.Attendees.All(s => video.Attendees.Contains(s)));
            Assert.True(dbVideo.Tags != null && dbVideo.Tags.All(s => video.Tags.Contains(s)));
            Assert.True(dbVideo.Relations != null && dbVideo.Relations.All(s => video.Relations.Contains(s)));
        }

        [Fact]
        public async Task GetVideos_Should_Get_All_Videos()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var videoRepository = new VideoRepository(memoryDatabaseProvider);

            IList<Video> videos = (await videoRepository.GetVideos()).ToList();

            foreach (Video dbVideo in _database.Videos.ToList())
            {
                Video video = videos.FirstOrDefault(m => m.Id == dbVideo.Id);

                Assert.NotNull(video);
                Assert.NotNull(dbVideo);
                Assert.Equal(video.Id, dbVideo.Id);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Slug, dbVideo.Slug);
                Assert.Equal(video.ShortDescription, dbVideo.ShortDescription);
                Assert.Equal(video.LongDescription, dbVideo.LongDescription);
                Assert.Equal(video.YoutubeUrl, dbVideo.YoutubeUrl);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Type, dbVideo.Type);
                Assert.Equal(video.PublishDate, dbVideo.PublishDate);
                Assert.True(dbVideo.Attendees != null && dbVideo.Attendees.All(s => video.Attendees.Contains(s)));
                Assert.True(dbVideo.Tags != null && dbVideo.Tags.All(s => video.Tags.Contains(s)));
                Assert.True(dbVideo.Relations != null && dbVideo.Relations.All(s => video.Relations.Contains(s)));
            }

            Assert.Equal(videos.Count, _database.Videos.Length);
            Assert.Contains(videos, video => _database.Videos.Select(v => v.Id).Contains(video.Id));
        }

        [Fact]
        public async Task GetVideosByIds_Should_Get_Videos_By_Given_Id_List()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var videoRepository = new VideoRepository(memoryDatabaseProvider);

            var ids = new[] { 1, 3 };

            IList<Video> videos = (await videoRepository.GetVideosByIds(ids)).ToList();
            List<Video> dbVideos = _database.Videos.Where(sponsor => ids.Contains(sponsor.Id)).ToList();

            foreach (Video dbVideo in dbVideos)
            {
                Video video = videos.FirstOrDefault(m => m.Id == dbVideo.Id);

                Assert.NotNull(video);
                Assert.NotNull(dbVideo);
                Assert.Equal(video.Id, dbVideo.Id);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Slug, dbVideo.Slug);
                Assert.Equal(video.ShortDescription, dbVideo.ShortDescription);
                Assert.Equal(video.LongDescription, dbVideo.LongDescription);
                Assert.Equal(video.YoutubeUrl, dbVideo.YoutubeUrl);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Type, dbVideo.Type);
                Assert.Equal(video.PublishDate, dbVideo.PublishDate);
                Assert.True(dbVideo.Attendees != null && dbVideo.Attendees.All(s => video.Attendees.Contains(s)));
                Assert.True(dbVideo.Tags != null && dbVideo.Tags.All(s => video.Tags.Contains(s)));
                Assert.True(dbVideo.Relations != null && dbVideo.Relations.All(s => video.Relations.Contains(s)));
            }

            Assert.Equal(videos.Count, dbVideos.Count);
            Assert.Contains(videos, video => ids.Contains(video.Id));
        }


        [Fact]
        public async Task GetVideosByType_Should_Get_Videos_By_Given_Type_List()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var videoRepository = new VideoRepository(memoryDatabaseProvider);

            string type = "Training";

            IList<Video> videos = (await videoRepository.GetVideosByType(type)).ToList();
            List<Video> dbVideos = _database.Videos.Where(sponsor => sponsor.Type == type).ToList();

            foreach (Video dbVideo in dbVideos)
            {
                Video video = videos.FirstOrDefault(m => m.Id == dbVideo.Id);

                Assert.NotNull(video);
                Assert.NotNull(dbVideo);
                Assert.Equal(video.Id, dbVideo.Id);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Slug, dbVideo.Slug);
                Assert.Equal(video.ShortDescription, dbVideo.ShortDescription);
                Assert.Equal(video.LongDescription, dbVideo.LongDescription);
                Assert.Equal(video.YoutubeUrl, dbVideo.YoutubeUrl);
                Assert.Equal(video.Title, dbVideo.Title);
                Assert.Equal(video.Type, dbVideo.Type);
                Assert.Equal(video.PublishDate, dbVideo.PublishDate);
                Assert.True(dbVideo.Attendees != null && dbVideo.Attendees.All(s => video.Attendees.Contains(s)));
                Assert.True(dbVideo.Tags != null && dbVideo.Tags.All(s => video.Tags.Contains(s)));
                Assert.True(dbVideo.Relations != null && dbVideo.Relations.All(s => video.Relations.Contains(s)));
            }

            Assert.Equal(videos.Count, dbVideos.Count);
            Assert.Contains(videos, video => video.Type == type);
        }
    }
}
