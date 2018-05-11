using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodefictionApi.Core.Contracts
{
    public interface IPodcastService
    {
        Task<IEnumerable<IPodcastModel>> GetPodcasts();

        Task<IPodcastModel> GetPodcastBySlug(string slug);

        Task<IEnumerable<IPodcastModel>> GetP2Ps();

        Task<IPodcastModel> GetP2PBySlug(string slug);

        Task<IEnumerable<IPodcastModel>> GetSpecials();

        Task<IPodcastModel> GetSpecialBySlug(string slug);
    }
}