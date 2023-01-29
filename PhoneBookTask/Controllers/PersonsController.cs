using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using PhoneBookTask.Dtos;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Models;

namespace PhoneBookTask.Controllers
{
    [RoutePrefix("api/persons")]
    public class PersonsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IPersonManager _personManager;

        public PersonsController(IMapper mapper, IPersonManager personManager)
        {
            _personManager = personManager;
            _mapper = mapper;
        }

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(DisplayPersonDto))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            var person = await _personManager.GetQuery().Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Person, DisplayPersonDto>(person));
        }

        [Route("")]
        [Route("search")]
        [HttpGet]
        [ResponseType(typeof(List<DisplayPersonDto>))]
        public IHttpActionResult GetAll([FromUri] string searchQuery = null)
        {
            var query = _personManager.GetQuery().Include(x => x.Company);

            var people = string.IsNullOrEmpty(searchQuery) ? query.ToList() : query.Where(x =>
                x.Address.Contains(searchQuery) || x.FullName.Contains(searchQuery) ||
                x.PhoneNumber.Contains(searchQuery) || x.Company.CompanyName.Contains(searchQuery)).ToList();

            var displayCompanies = _mapper.Map<List<Person>, List<DisplayPersonDto>>(people);
            return Ok(displayCompanies);
        }

        [Route("random")]
        [HttpGet]
        [ResponseType(typeof(DisplayPersonDto))]
        public IHttpActionResult Random()
        {
            var randomPerson = _personManager.GetQuery()
                .OrderBy(r => Guid.NewGuid())
                .FirstOrDefault();

            return Ok(_mapper.Map<Person, DisplayPersonDto>(randomPerson));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DisplayPersonDto))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _personManager.Save(person);

            return Ok(_mapper.Map<Person, DisplayPersonDto>(person));
        }

        [Route("")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            try
            {
                await _personManager.Update(person);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_personManager.GetQuery().Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpDelete]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            var person = _personManager.GetQuery().FirstOrDefault(x => x.Id == id);

            if (person == default)
            {
                return NotFound();
            }

            await _personManager.Delete(person);
            return Ok(person);
        }
    }
}