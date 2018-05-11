using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Codefiction.CodefictionTech.CodefictionApi.Server.ApiControllers
{
    [Route("api/P2Ps")]
    public class P2PController : Controller
    {
        private readonly IPodcastService _podcastService;

        public P2PController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> P2Ps()
        {
            IEnumerable<IPodcastModel> p2ps = await _podcastService.GetP2Ps();

            return Ok(p2ps);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> P2PBySlug(string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPodcastModel p2p = await _podcastService.GetP2PBySlug(slug);

            if (p2p == null)
            {
                return NotFound();
            }        

            return Ok(p2p);
        }        
    }
}
