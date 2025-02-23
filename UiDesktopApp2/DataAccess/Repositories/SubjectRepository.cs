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
    public class SubjectRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<List<SubjectDTO>> GetAllSubjects()
        {
            var subjects = await context.Subjects
                .Include(p => p.Results)
                .ThenInclude(r => r.ImageSetTimes)
                .Include(p => p.Results)
                .ToListAsync();

            return mapper.Map<List<SubjectDTO>>(subjects);
        }

        public async Task<int> CreateSubject(SubjectDTO subjcetDto)
        {
            var person = mapper.Map<Subject>(subjcetDto);
            context.Subjects.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }
    }
}
