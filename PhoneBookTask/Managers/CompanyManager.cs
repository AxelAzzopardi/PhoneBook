using System.Linq;
using System.Threading.Tasks;
using PhoneBookTask.Context;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Models;

namespace PhoneBookTask.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly PhoneBookContext _context;

        public CompanyManager(PhoneBookContext context)
        {
            _context = context;
        }

        public IQueryable<Company> GetQuery()
        {
            return _context.Companies.AsQueryable();
        }

        public async Task<int> Save(Company company)
        {
            _context.Companies.Add(company);
            return await _context.SaveChangesAsync();
        }
    }
}