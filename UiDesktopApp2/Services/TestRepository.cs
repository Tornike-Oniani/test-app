using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Windows.Documents;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Services
{
    public class TestRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<List<TestDTO>> GetAllTestsAsync()
        {
            var tests = await context.Tests
                .Include(t => t.ImageSets)
                    .ThenInclude(s => s.ImageVariants)
                .ToListAsync();

            return mapper.Map<List<TestDTO>>(tests);
        }

        public async Task<int> CreateTest(TestDTO test)
        {
            var newTest = new Test()
            {
                Name = test.Name,
            };

            context.Tests.Add(newTest);

            await context.SaveChangesAsync();
            return newTest.Id;
        }

        public async Task UpdateTest(TestDTO testDto)
        {
            var existingTest = await context.Tests.FindAsync(testDto.Id);


            var test = mapper.Map(testDto, existingTest);
            await context.SaveChangesAsync();
        }
    }
}
