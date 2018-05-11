using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Codefiction.CodefictionTech.CodefictionApi.Server.ApiControllers
{
    [Route("api/podcasts")]
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;

        public PodcastController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Podcasts()
        {
            IEnumerable<IPodcastModel> podcastModels = await _podcastService.GetPodcasts();

            return Ok(podcastModels);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> PodcastBySlug(string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPodcastModel podcastModel = await _podcastService.GetPodcastBySlug(slug);

            if (podcastModel == null)
            {
                return NotFound();
            }

            return Ok(podcastModel);
        }        
    }
}
