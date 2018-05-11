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
    public class VideoServiceTests
    {
        [Fact]
        public async Task GetVideoById_Should_Return_Call_IVideoRepository_GetVideoById()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var id = 1;

            mock.VideoRepository
                .Setup(repository => repository.GetVideoById(It.Is<int>(i => i == id)))
                .ReturnsAsync(() => new Video());

            mock.VideoModelMapper.Setup(mapper => mapper.Map(It.IsAny<Video>()))
                .ReturnsAsync(() => new VideoModel());

            VideoModel videoModel = await mock.GetVideoById(id);

            mock.VideoRepository.Verify(repository => repository.GetVideoById(It.IsAny<int>()), Times.Once);
            Assert.NotNull(videoModel);
        }

        [Fact]
        public async Task GetPodcasts_Should_Return_Call_IVideoRepository_Map()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var id = 1;

            var video = new Video() {Id = id, Title = "Akka Live Coding - Mert Susur - Deniz İrgin"};

            mock.VideoRepository
                .Setup(repository => repository.GetVideoById(It.IsAny<int>()))
                .ReturnsAsync(() => video);

            mock.VideoModelMapper
                .Setup(mapper => mapper.Map(It.Is<Video>(v => v.Id == video.Id && v.Title == video.Title)))
                .ReturnsAsync(() => new VideoModel());

            VideoModel videoModel = await mock.GetVideoById(id);

            mock.VideoModelMapper.Verify(repository => repository.Map(It.IsAny<Video>()), Times.Once);
            Assert.NotNull(videoModel);
        }

        [Fact]
        public async Task GetVideosByType_Should_Throw_ArgumentNullException_If_Type_Is_Null()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            string type = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => mock.GetVideosByType(type));
            mock.VideoRepository.Verify(repository => repository.GetVideosByType(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetVideosByType_Should_Return_Call_IVideoRepository_GetVideosByType()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var type = "Meetup";

            mock.VideoRepository
                .Setup(repository => repository.GetVideosByType(It.Is<string>(t => t == type)))
                .ReturnsAsync(() => new List<Video>());

            mock.VideoModelMapper.Setup(mapper => mapper.Map(It.IsAny<IEnumerable<Video>>()))
                .ReturnsAsync(() => new List<VideoModel>());

            IEnumerable<VideoModel> videoModels = await mock.GetVideosByType(type);

            mock.VideoRepository.Verify(repository => repository.GetVideosByType(It.IsAny<string>()), Times.Once);
            Assert.NotNull(videoModels);
        }

        [Fact]
        public async Task GetVideosByType_Should_Return_Call_IVideoRepository_Map()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var type = "Meetup";

            var videos = new List<Video>()
            {
                new Video() {Id = 1, Title = "Akka Live Coding - Mert Susur - Deniz İrgin"},
                new Video() {Id = 2, Title = "Ethereum 101 - Mert Susur"}
            };

            mock.VideoRepository
                .Setup(repository => repository.GetVideosByType(It.IsAny<string>()))
                .ReturnsAsync(() => videos);

            mock.VideoModelMapper
                .Setup(mapper => mapper.Map(It.Is<IEnumerable<Video>>(v => v.Equals(videos))))
                .ReturnsAsync(() => new List<VideoModel>());

            IEnumerable<VideoModel> videoModels = await mock.GetVideosByType(type);

            mock.VideoModelMapper.Verify(repository => repository.Map(It.IsAny<IEnumerable<Video>>()), Times.Once);
            Assert.NotNull(videoModels);
        }

        [Fact]
        public async Task GetVideosByIds_Should_Throw_ArgumentNullException_If_Type_Is_Null()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            List<int> ids = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => mock.GetVideosByIds(ids));
            mock.VideoRepository.Verify(repository => repository.GetVideosByIds(It.IsAny<IList<int>>()), Times.Never);
        }

        [Fact]
        public async Task GetVideosByIds_Should_Return_Call_IVideoRepository_GetVideosByIds()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var ids = new List<int>() {1, 3, 5};

            mock.VideoRepository
                .Setup(repository => repository.GetVideosByIds(It.Is<IList<int>>(t => t.All(i => ids.Contains(i)))))
                .ReturnsAsync(() => new List<Video>());

            mock.VideoModelMapper.Setup(mapper => mapper.Map(It.IsAny<IEnumerable<Video>>()))
                .ReturnsAsync(() => new List<VideoModel>());

            IEnumerable<VideoModel> videoModels = await mock.GetVideosByIds(ids);

            mock.VideoRepository.Verify(repository => repository.GetVideosByIds(It.IsAny<IList<int>>()), Times.Once);
            Assert.NotNull(videoModels);
        }

        [Fact]
        public async Task GetVideosByIds_Should_Return_Call_IVideoRepository_Map()
        {
            VideoServiceMock mock = VideoServiceMock.Create();

            var ids = new List<int>() { 1, 3, 5 };

            mock.VideoRepository
                .Setup(repository => repository.GetVideosByIds(It.IsAny<IList<int>>()))
                .ReturnsAsync(() => new List<Video>());

            mock.VideoModelMapper
                .Setup(mapper => mapper.Map((It.IsAny<IEnumerable<Video>>())))
                .ReturnsAsync(() => new List<VideoModel>());

            IEnumerable<VideoModel> videoModels = await mock.GetVideosByIds(ids);

            mock.VideoModelMapper.Verify(repository => repository.Map(It.IsAny<IEnumerable<Video>>()), Times.Once);
            Assert.NotNull(videoModels);
        }
    }
}
