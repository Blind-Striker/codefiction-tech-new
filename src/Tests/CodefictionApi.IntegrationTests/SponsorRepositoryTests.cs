using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Repositories;
using Xunit;

namespace CodefictionApi.IntegrationTests
{
    public class SponsorRepositoryTests
    {
        private readonly Database _database;

        public SponsorRepositoryTests()
        {
            _database = new Database()
            {
                Sponsors = new[]
                {
                    new Sponsor() {Id = 1, Name = "armut.com", LogoUrl = "armut.png"},
                    new Sponsor() {Id = 2, Name = "Tekna", LogoUrl = "tekna.png"},
                    new Sponsor() {Id = 3, Name = "Microsoft", LogoUrl = "microsoft.png"}
                }
            };
        }

        [Fact]
        public async Task GetSponsorById_Should_Get_Sponsor_By_Given_Id()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var sponsorRepository = new SponsorRepository(memoryDatabaseProvider);

            var id = 1;

            Sponsor sponsor = await sponsorRepository.GetSponsorById(id);
            Sponsor dbSponsor = _database.Sponsors.FirstOrDefault(m => m.Id == id);

            Assert.NotNull(sponsor);
            Assert.NotNull(dbSponsor);
            Assert.Equal(sponsor.Id, dbSponsor.Id);
            Assert.Equal(sponsor.Name, dbSponsor.Name);
            Assert.Equal(sponsor.LogoUrl, dbSponsor.LogoUrl);
        }

        [Fact]
        public async Task GetSponsorByName_Should_Get_Sponsor_By_Given_Name()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var sponsorRepository = new SponsorRepository(memoryDatabaseProvider);

            var name = "Tekna";

            Sponsor sponsor = await sponsorRepository.GetSponsorByName(name);
            Sponsor dbSponsor = _database.Sponsors.FirstOrDefault(m => m.Name == name);

            Assert.NotNull(sponsor);
            Assert.NotNull(dbSponsor);
            Assert.Equal(sponsor.Id, dbSponsor.Id);
            Assert.Equal(sponsor.Name, dbSponsor.Name);
            Assert.Equal(sponsor.LogoUrl, dbSponsor.LogoUrl);
        }

        [Fact]
        public async Task GetMeetups_Should_Get_All_Sponsors()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var sponsorRepository = new SponsorRepository(memoryDatabaseProvider);

            IList<Sponsor> sponsors = (await sponsorRepository.GetSponsors()).ToList();

            foreach (Sponsor dbSponsor in _database.Sponsors.ToList())
            {
                Sponsor sponsor = sponsors.FirstOrDefault(m => m.Id == dbSponsor.Id);

                Assert.NotNull(sponsor);
                Assert.NotNull(dbSponsor);
                Assert.Equal(sponsor.Id, dbSponsor.Id);
                Assert.Equal(sponsor.Name, dbSponsor.Name);
                Assert.Equal(sponsor.LogoUrl, dbSponsor.LogoUrl);
            }

            Assert.Equal(sponsors.Count, _database.Sponsors.Length);
            Assert.Contains(sponsors, sponsor => _database.Sponsors.Select(s => s.Id).Contains(sponsor.Id));
        }

        [Fact]
        public async Task GetMeetupsByIds_Should_Get_Sponsors_By_Given_Id_List()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var sponsorRepository = new SponsorRepository(memoryDatabaseProvider);

            var ids = new[] {1, 3};

            IList<Sponsor> sponsors = (await sponsorRepository.GetSponsorsByIds(ids)).ToList();
            IList<Sponsor> dbSponsors = _database.Sponsors.Where(sponsor => ids.Contains(sponsor.Id)).ToList();

            foreach (Sponsor dbSponsor in dbSponsors)
            {
                Sponsor sponsor = sponsors.FirstOrDefault(m => m.Id == dbSponsor.Id);

                Assert.NotNull(sponsor);
                Assert.NotNull(dbSponsor);
                Assert.Equal(sponsor.Id, dbSponsor.Id);
                Assert.Equal(sponsor.Name, dbSponsor.Name);
                Assert.Equal(sponsor.LogoUrl, dbSponsor.LogoUrl);
            }

            Assert.Equal(sponsors.Count, dbSponsors.Count);
            Assert.Contains(sponsors, sponsor => ids.Contains(sponsor.Id));
        }
    }
}
