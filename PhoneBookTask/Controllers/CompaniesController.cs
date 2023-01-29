using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using PhoneBookTask.Dtos;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Models;

namespace PhoneBookTask.Controllers
{
    [RoutePrefix("api/companies")]
    public class CompaniesController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;

        public CompaniesController(IMapper mapper, ICompanyManager companyManager)
        {
            _companyManager = companyManager;
            _mapper = mapper;
        }

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(DisplayCompanyDto))]
        public async Task<IHttpActionResult> GetCompany(int id)
        {
            var company = await _companyManager.GetQuery().Include(x => x.Persons).FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Company, DisplayCompanyDto>(company));
        }

        [Route("")]
        [Route("all")]
        [HttpGet]
        [ResponseType(typeof(List<DisplayCompanyDto>))]
        public IHttpActionResult GetAll()
        {
            var companies = _companyManager.GetQuery().Include(x => x.Persons).ToList();
            var displayCompanies = _mapper.Map<List<Company>, List<DisplayCompanyDto>>(companies);
            return Ok(displayCompanies);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DisplayCompanyDto))]
        public async Task<IHttpActionResult> PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_companyManager.GetQuery().Any(c => c.CompanyName == company.CompanyName))
            {
                return BadRequest("Company Name is already in use");
            }

            await _companyManager.Save(company);

            return Ok(_mapper.Map<Company, DisplayCompanyDto>(company));
        }
    }
}