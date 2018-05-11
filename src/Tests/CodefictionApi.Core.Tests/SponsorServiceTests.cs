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
    public class SponsorServiceTests
    {
        [Fact]
        public async Task GetSponsorById_Should_Call_ISponsorRepository_GetSponsorById()
        {
            SponsorServiceMock mock = SponsorServiceMock.Create();

            var id = 1;

            mock.SponsorRepository
                .Setup(repository => repository.GetSponsorById(It.Is<int>(i => i == id)))
                .ReturnsAsync(() => new Sponsor() { Id = id, Name = "armut.com" });

            Sponsor sponsor = await mock.GetSponsorById(id);

            mock.SponsorRepository.Verify(repository => repository.GetSponsorById(It.IsAny<int>()), Times.Once);
            Assert.NotNull(sponsor);
        }

        [Fact]
        public async Task GetSponsorByName_Should_Throw_ArgumentNullException_If_Name_Is_Null()
        {
            SponsorServiceMock mock = SponsorServiceMock.Create();

            string name = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => mock.GetSponsorByName(name));
            mock.SponsorRepository.Verify(repository => repository.GetSponsorByName(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetSponsorByName_Should_Call_ISponsorRepository_GetSponsorByName()
        {
            SponsorServiceMock mock = SponsorServiceMock.Create();

            var name = "armut.com";

            mock.SponsorRepository
                .Setup(repository => repository.GetSponsorByName(It.Is<string>(s => s == name)))
                .ReturnsAsync(() => new Sponsor() { Id = 1, Name = name });

            Sponsor sponsor = await mock.GetSponsorByName(name);

            mock.SponsorRepository.Verify(repository => repository.GetSponsorByName(It.IsAny<string>()), Times.Once);
            Assert.NotNull(sponsor);
        }

        [Fact]
        public async Task GetSponsors_Should_Call_ISponsorRepository_GetSponsors()
        {
            SponsorServiceMock mock = SponsorServiceMock.Create();

            mock.SponsorRepository
                .Setup(repository => repository.GetSponsors())
                .ReturnsAsync(() => new List<Sponsor>());

            IEnumerable<Sponsor> sponsors = await mock.GetSponsors();

            mock.SponsorRepository.Verify(repository => repository.GetSponsors(), Times.Once);
            Assert.NotNull(sponsors);
        }

        [Fact]
        public async Task GetSponsorsByIds_Should_Call_ISponsorRepository_GetSponsorsByIds()
        {
            SponsorServiceMock mock = SponsorServiceMock.Create();

            IList<int> ids = new List<int>() {1, 2, 4};

            mock.SponsorRepository
                .Setup(repository => repository.GetSponsorsByIds(It.Is<IList<int>>(i => i.All(i1 => ids.Contains(i1)))))
                .ReturnsAsync(() => new List<Sponsor>());

            IEnumerable<Sponsor> sponsors = await mock.GetSponsorsByIds(ids);

            mock.SponsorRepository.Verify(repository => repository.GetSponsorsByIds(It.IsAny<IList<int>>()), Times.Once);
            Assert.NotNull(sponsors);
        }
    }
}
