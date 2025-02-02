using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;

namespace UiDesktopApp2.Services
{
    public class PersonRepository(ApplicationDbContext context)
    {
        public async Task<int> CreatePerson(Person person)
        {
            context.People.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }
    }
}
