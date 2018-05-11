using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;

namespace CodefictionApi.Core.Services
{
    public class PodcastService : IPodcastService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IPodcastModelMapper _podcastModelMapper;

        public PodcastService(IPodcastRepository podcastRepository, IPodcastModelMapper podcastModelMapper)
        {
            _podcastRepository = podcastRepository;
            _podcastModelMapper = podcastModelMapper;
        }

        public async Task<IEnumerable<IPodcastModel>> GetPodcasts()
        {
            IEnumerable<Podcast> podcasts = await _podcastRepository.GetPodcasts();
            IEnumerable<IPodcastModel> podcastModels = await _podcastModelMapper.Map(podcasts);

            return podcastModels;
        }

        public async Task<IPodcastModel> GetPodcastBySlug(string slug)
        {
            if (slug == null)
            {
                throw new ArgumentNullException(nameof(slug));
            }

            Podcast podcast = await _podcastRepository.GetPodcastBySlug(slug);
            IPodcastModel podcastModel = await _podcastModelMapper.Map(podcast);

            return podcastModel;
        }

        public async Task<IEnumerable<IPodcastModel>> GetP2Ps()
        {
            IEnumerable<P2P> p2ps = await _podcastRepository.GetP2Ps();
            IEnumerable<IPodcastModel> podcastModels = await _podcastModelMapper.Map(p2ps);

            return podcastModels;
        }

        public async Task<IPodcastModel> GetP2PBySlug(string slug)
        {
            if (slug == null)
            {
                throw new ArgumentNullException(nameof(slug));
            }

            P2P p2p = await _podcastRepository.GetP2PBySlug(slug);
            IPodcastModel podcastModel = await _podcastModelMapper.Map(p2p);

            return podcastModel;
        }

        public async Task<IEnumerable<IPodcastModel>> GetSpecials()
        {
            IEnumerable<Special> specials = await _podcastRepository.GetSpecials();
            IEnumerable<IPodcastModel> podcastModels = await _podcastModelMapper.Map(specials);

            return podcastModels;
        }

        public async Task<IPodcastModel> GetSpecialBySlug(string slug)
        {
            if (slug == null)
            {
                throw new ArgumentNullException(nameof(slug));
            }

            Special special = await _podcastRepository.GetSpecialBySlug(slug);
            IPodcastModel podcastModel = await _podcastModelMapper.Map(special);

            return podcastModel;
        }
    }
}
