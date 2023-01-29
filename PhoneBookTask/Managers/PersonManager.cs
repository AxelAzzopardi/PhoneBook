using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PhoneBookTask.Context;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Models;

namespace PhoneBookTask.Managers
{
    public class PersonManager : IPersonManager
    {
        private readonly PhoneBookContext _context;

        public PersonManager(PhoneBookContext context)
        {
            _context = context;
        }

        public IQueryable<Person> GetQuery()
        {
            return _context.Persons.AsQueryable();
        }

        public async Task<int> Save(Person person)
        {
            _context.Persons.Add(person);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Person person, EntityState state = EntityState.Modified)
        {
            _context.Entry(person).State = state;
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(Person person)
        {
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}