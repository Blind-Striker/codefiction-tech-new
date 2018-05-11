using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;
using Microsoft.AspNetCore.Mvc;

namespace Codefiction.CodefictionTech.CodefictionApi.Server.ApiControllers
{
    [Route("api/people")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Route("type/{type}")]
        public async Task<IActionResult> PeopleByType(string type)
        {
            IEnumerable<Person> people = new List<Person>();

            if (type.Equals("Crew", StringComparison.InvariantCultureIgnoreCase))
            {
                people = await _personService.GetCrew();
            }
            else if (type.Equals("Guest", StringComparison.InvariantCultureIgnoreCase))
            {
                people = await _personService.GetGuests();
            }

            return Ok(people);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<IActionResult> PersonByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Person person = await _personService.GetPersonByName(name);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> PeopleByNames([FromQuery]IList<string> names)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Person> persons = await _personService.GetPeopleByNames(names);

            return Ok(persons);
        }
    }
}