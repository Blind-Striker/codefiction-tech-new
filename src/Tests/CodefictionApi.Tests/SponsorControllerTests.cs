using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CodefictionApi.Tests
{
    public class SponsorControllerTests
    {
        [Fact]
        public async Task Specials_Should_Call_ISponsorService_GetSponsors_And_Return_Ok()
        {
            SponsorControllerMock mock = SponsorControllerMock.Create();

            mock.SponsorService.Setup(service => service.GetSponsors()).ReturnsAsync(() => new List<Sponsor>());

            IActionResult actionResult = await mock.Sponsors();

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var sponsors = okObjectResult.Value as IEnumerable<Sponsor>;

            Assert.NotNull(sponsors);
            mock.SponsorService.Verify(service => service.GetSponsors(), Times.Once);
        }

        [Fact]
        public async Task SponsorById_Should_Return_BadRequest_If_ModelState_Is_Invalid()
        {
            SponsorControllerMock mock = SponsorControllerMock.Create();
            mock.ModelState.AddModelError("test", "test");

            IActionResult actionResult = await mock.SponsorById(1);

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var serializableError = badRequestObjectResult.Value as SerializableError;

            Assert.NotNull(serializableError);
            Assert.True(((string[])serializableError["test"])[0] == "test");
            mock.SponsorService.Verify(service => service.GetSponsorById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SponsorById_Should_Call_ISponsorService_GetSponsorById_And_Return_NotFound_If_ISponsorService_GetSponsorById_Returns_Null()
        {
            SponsorControllerMock mock = SponsorControllerMock.Create();

            mock.SponsorService.Setup(service => service.GetSponsorById(It.IsAny<int>())).ReturnsAsync(() => null);

            IActionResult actionResult = await mock.SponsorById(1);

            var notFoundResult = actionResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            mock.SponsorService.Verify(service => service.GetSponsorById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task SponsorById_Should_Call_ISponsorService_GetSponsorById_And_Return_Ok()
        {
            SponsorControllerMock mock = SponsorControllerMock.Create();

            var id = 1;

            mock.SponsorService.Setup(service => service.GetSponsorById(It.Is<int>(i => i == id))).ReturnsAsync(() => new Sponsor());

            IActionResult actionResult = await mock.SponsorById(id);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var sponsorModel = okObjectResult.Value as Sponsor;

            Assert.NotNull(sponsorModel);
            mock.SponsorService.Verify(service => service.GetSponsorById(It.IsAny<int>()), Times.Once);
        }
    }
}
