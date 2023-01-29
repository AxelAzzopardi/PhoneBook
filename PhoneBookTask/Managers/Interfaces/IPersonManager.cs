using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PhoneBookTask.Models;

namespace PhoneBookTask.Managers.Interfaces
{
    public interface IPersonManager
    {
        IQueryable<Person> GetQuery();
        Task<int> Save(Person person);
        Task<int> Update(Person person, EntityState state = EntityState.Modified);
        Task Delete(Person person);
    }
}