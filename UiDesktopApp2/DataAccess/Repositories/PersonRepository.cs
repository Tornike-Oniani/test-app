using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.DataAccess.Repositories
{
    public class PersonRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<List<PersonDTO>> GetAllPeopleAsync()
        {
            var subjects = await context.People
                .Include(p => p.Results)
                .ThenInclude(r => r.ImageSetTimes)
                .Include(p => p.Results)
                .ToListAsync();

            return mapper.Map<List<PersonDTO>>(subjects);
        }

        public async Task<int> CreatePerson(PersonDTO personDto)
        {
            var person = mapper.Map<Person>(personDto);
            context.People.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }
    }
}
