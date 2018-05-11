using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Codefiction.CodefictionTech.CodefictionApi.Server.ApiControllers
{
    [Route("api/specials")]
    public class SpecialController : Controller
    {
        private readonly IPodcastService _podcastService;

        public SpecialController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Specials()
        {
            IEnumerable<IPodcastModel> specials = await _podcastService.GetSpecials();

            return Ok(specials);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> SpecialBySlug(string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPodcastModel specialModel = await _podcastService.GetSpecialBySlug(slug);

            if (specialModel == null)
            {
                return NotFound();
            }

            return Ok(specialModel);
        }        
    }
}
