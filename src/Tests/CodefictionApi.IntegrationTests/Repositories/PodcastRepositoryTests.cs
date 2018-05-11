using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodefictionApi.Core.Data;
using CodefictionApi.Core.Repositories;
using Xunit;

namespace CodefictionApi.IntegrationTests.Repositories
{
    public class PodcastRepositoryTests
    {
        private readonly Database _database;

        public PodcastRepositoryTests()
        {
            _database = new Database()
            {
                Podcasts = new[]
                {
                    new Podcast()
                    {
                        Id = 1,
                        Season = 1,
                        Title = "Ondorduncu Bolum - Algoritmalar ve is gorusmeleri",
                        Slug = "ondorduncu-bolum-algoritmalar-ve-is-gorusmeleri",
                        SoundcloudId = "soundcloud/1",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-Xqax3MOuPg",
                        ShortDescription = "Bu video'da Solidity dilini kullanarak akilli kontrat nasil hazirlanir anlatmaya calistik.",
                        LongDescription = "",
                        Guest = null,
                        Attendees =
                            new[]
                            {
                                "Barış Özaydın", "Deniz İrgin", "Deniz Özgen", "Fırat Özbolat", "Mahmut Gündoğdu",
                                "Uğur Atar", "Mert Susur"
                            },
                        Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                        Tags = new[] {"interviews"},
                        PublishDate = DateTime.Now,
                    },
                    new Podcast()
                    {
                        Id = 13,
                        Season = 1,
                        Title = "Codefiction ve Komünite Değerleri",
                        Slug = "onucuncu-bolum-codefiction-ve-komunite-degerleri",
                        SoundcloudId = "soundcloud/13",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-Xqax3MOuPg",
                        ShortDescription = "Bu bölümde tren yolculuğu sırasında Codefiction komünitesinin değerlerini tartıştık.",
                        LongDescription = "",
                        Guest = "Eray Acar",
                        Attendees =
                            new[]
                            {
                                "Barış Özaydın", "Deniz İrgin", "Deniz Özgen", "Fırat Özbolat", "Uğur Atar",
                                "Mert Susur"
                            },
                        Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                        Tags = new[] {"codefiction"},
                        PublishDate = DateTime.Now,
                    },
                    new Podcast()
                    {
                        Id = 17,
                        Season = 1,
                        Title = "Güvenlik",
                        Slug = "onyedinci-bolum-guvenlik",
                        SoundcloudId = "soundcloud/17",
                        YoutubeUrl = "https://www.youtube.com/watch?v=-Xqax3MOuPg",
                        ShortDescription = "Yoğunluk ve biraz da uyuşukluk sebebiyle üç hafta önce kaydettiğimiz yayınımızı ancak yükleyebiliyoruz.",
                        LongDescription = "",
                        Guest = null,
                        Attendees =
                            new[]
                            {
                                "Ahmet Erdem Kahveci",
                                "Fırat Özbolat",
                                "Uğur Atar",
                                "Mert Susur"
                            },
                        Relations = new[] {new Relation() {Id = 1, Type = "medium"}},
                        Tags = new[] {"security"},
                        PublishDate = DateTime.Now,
                    }
                }
            };
        }

        [Fact]
        public async Task GetPodcastById_Should_Get_Podcast_By_Given_Id()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var podcastRepository = new PodcastRepository(memoryDatabaseProvider);

            var id = 1;

            Podcast podcast = await podcastRepository.GetPodcastById(id);
            Podcast dbPodcast = _database.Podcasts.FirstOrDefault(p => p.Id == id);

            Assert.NotNull(podcast);
            Assert.NotNull(dbPodcast);
            Assert.Equal(podcast.Id, dbPodcast.Id);
            Assert.Equal(podcast.Season, dbPodcast.Season);
            Assert.Equal(podcast.Title, dbPodcast.Title);
            Assert.Equal(podcast.Slug, dbPodcast.Slug);
            Assert.Equal(podcast.SoundcloudId, dbPodcast.SoundcloudId);
            Assert.Equal(podcast.YoutubeUrl, dbPodcast.YoutubeUrl);
            Assert.Equal(podcast.ShortDescription, dbPodcast.ShortDescription);
            Assert.Equal(podcast.LongDescription, dbPodcast.LongDescription);
            Assert.Equal(podcast.Guest, dbPodcast.Guest);
            Assert.Equal(podcast.PublishDate, dbPodcast.PublishDate);
            Assert.True(podcast.Attendees != null && podcast.Attendees.All(s => podcast.Attendees.Contains(s)));
            Assert.True(podcast.Tags != null && podcast.Tags.All(s => podcast.Tags.Contains(s)));
            Assert.True(podcast.Relations != null && podcast.Relations.All(s => podcast.Relations.Contains(s)));
        }

        [Fact]
        public async Task GetPodcastBySlug_Should_Get_Podcast_By_Given_Slug()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var podcastRepository = new PodcastRepository(memoryDatabaseProvider);

            var slug = "onucuncu-bolum-codefiction-ve-komunite-degerleri";

            Podcast podcast = await podcastRepository.GetPodcastBySlug(slug);
            Podcast dbPodcast = _database.Podcasts.FirstOrDefault(p => p.Slug == slug);

            Assert.NotNull(podcast);
            Assert.NotNull(dbPodcast);
            Assert.Equal(podcast.Id, dbPodcast.Id);
            Assert.Equal(podcast.Season, dbPodcast.Season);
            Assert.Equal(podcast.Title, dbPodcast.Title);
            Assert.Equal(podcast.Slug, dbPodcast.Slug);
            Assert.Equal(podcast.SoundcloudId, dbPodcast.SoundcloudId);
            Assert.Equal(podcast.YoutubeUrl, dbPodcast.YoutubeUrl);
            Assert.Equal(podcast.ShortDescription, dbPodcast.ShortDescription);
            Assert.Equal(podcast.LongDescription, dbPodcast.LongDescription);
            Assert.Equal(podcast.Guest, dbPodcast.Guest);
            Assert.Equal(podcast.PublishDate, dbPodcast.PublishDate);
            Assert.True(podcast.Attendees != null && podcast.Attendees.All(s => podcast.Attendees.Contains(s)));
            Assert.True(podcast.Tags != null && podcast.Tags.All(s => podcast.Tags.Contains(s)));
            Assert.True(podcast.Relations != null && podcast.Relations.All(s => podcast.Relations.Contains(s)));
        }

        [Fact]
        public async Task GetPodcasts_Should_Get_All_Podcasts()
        {
            var memoryDatabaseProvider = new InMemoryDatabaseProvider(_database);
            var podcastRepository = new PodcastRepository(memoryDatabaseProvider);

            IList<Podcast> podcasts = (await podcastRepository.GetPodcasts()).ToList();

            foreach (Podcast dbPodcast in _database.Podcasts.ToList())
            {
                Podcast podcast = podcasts.FirstOrDefault(m => m.Id == dbPodcast.Id);

                Assert.NotNull(podcast);
                Assert.NotNull(dbPodcast);
                Assert.Equal(podcast.Id, dbPodcast.Id);
                Assert.Equal(podcast.Season, dbPodcast.Season);
                Assert.Equal(podcast.Title, dbPodcast.Title);
                Assert.Equal(podcast.Slug, dbPodcast.Slug);
                Assert.Equal(podcast.SoundcloudId, dbPodcast.SoundcloudId);
                Assert.Equal(podcast.YoutubeUrl, dbPodcast.YoutubeUrl);
                Assert.Equal(podcast.ShortDescription, dbPodcast.ShortDescription);
                Assert.Equal(podcast.LongDescription, dbPodcast.LongDescription);
                Assert.Equal(podcast.Guest, dbPodcast.Guest);
                Assert.Equal(podcast.PublishDate, dbPodcast.PublishDate);
                Assert.True(podcast.Attendees != null && podcast.Attendees.All(s => podcast.Attendees.Contains(s)));
                Assert.True(podcast.Tags != null && podcast.Tags.All(s => podcast.Tags.Contains(s)));
                Assert.True(podcast.Relations != null && podcast.Relations.All(s => podcast.Relations.Contains(s)));
            }

            Assert.Equal(podcasts.Count, _database.Podcasts.Length);
            Assert.Contains(podcasts, video => _database.Podcasts.Select(v => v.Id).Contains(video.Id));
        }
    }
}
