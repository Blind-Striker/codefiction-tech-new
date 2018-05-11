using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Models;
using CodefictionApi.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CodefictionApi.Tests
{
    public class VideoControllerTests
    {
        [Fact]
        public async Task VideosByType_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            VideoControllerMock mock = VideoControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.VideosByType("Meetup");

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.VideoService.Verify(service => service.GetVideosByType(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task VideosByType_Should_Call_IVideoService_GetVideosByType_And_Return_Ok()
        {
            VideoControllerMock mock = VideoControllerMock.Create();

            var videoType = "Meetup";

            mock.VideoService.Setup(service => service.GetVideosByType(It.Is<string>(i => i == videoType))).ReturnsAsync(() => new List<VideoModel>());

            IActionResult actionResult = await mock.VideosByType(videoType);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var videoModels = okObjectResult.Value as IEnumerable<VideoModel>;

            Assert.NotNull(videoModels);
            mock.VideoService.Verify(service => service.GetVideosByType(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task VideosByIds_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            VideoControllerMock mock = VideoControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.VideosByIds(new List<int>() {1, 3, 5});

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Never);
        }

        [Fact]
        public async Task VideosByIds_Should_Call_IVideoService_GetVideosByIds_And_Return_Ok()
        {
            VideoControllerMock mock = VideoControllerMock.Create();

            IList<int> ids = new List<int>() {1, 2, 5};

            mock.VideoService.Setup(service => service.GetVideosByIds(It.Is<IList<int>>(vids => vids.All(i => ids.Contains(i))))).ReturnsAsync(() => new List<VideoModel>());

            IActionResult actionResult = await mock.VideosByIds(ids);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var videoModels = okObjectResult.Value as IEnumerable<VideoModel>;

            Assert.NotNull(videoModels);
            mock.VideoService.Verify(service => service.GetVideosByIds(It.IsAny<IList<int>>()), Times.Once);
        }

        [Fact]
        public async Task VideoById_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            VideoControllerMock mock = VideoControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.VideoById(1);

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.VideoService.Verify(service => service.GetVideoById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task VideoById_Should_Call_IVideoService_GetVideoById_And_Return_NotFound_If_IVideoService_GetVideoById_Returns_Null()
        {
            VideoControllerMock mock = VideoControllerMock.Create();

            mock.VideoService.Setup(service => service.GetVideoById(It.IsAny<int>())).ReturnsAsync(() => null);

            IActionResult actionResult = await mock.VideoById(1);

            var notFoundResult = actionResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            mock.VideoService.Verify(service => service.GetVideoById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task VideoById_Should_Call_IVideoService_GetVideoById_And_Return_Ok()
        {
            VideoControllerMock mock = VideoControllerMock.Create();

            var id = 1;

            mock.VideoService.Setup(service => service.GetVideoById(It.Is<int>(i => i == id))).ReturnsAsync(() => new VideoModel());

            IActionResult actionResult = await mock.VideoById(id);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var videoModel = okObjectResult.Value as VideoModel;

            Assert.NotNull(videoModel);
            mock.VideoService.Verify(service => service.GetVideoById(It.IsAny<int>()), Times.Once);
        }
    }
}
