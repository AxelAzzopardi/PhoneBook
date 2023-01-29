using System.Data.Entity;
using PhoneBookTask.Models;

namespace PhoneBookTask.Context
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext() : base("name=PhoneBookContext")
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}