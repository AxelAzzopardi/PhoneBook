using System.Linq;
using System.Threading.Tasks;
using PhoneBookTask.Models;

namespace PhoneBookTask.Managers.Interfaces
{
    public interface ICompanyManager
    {
        IQueryable<Company> GetQuery();
        Task<int> Save(Company company);
    }
}