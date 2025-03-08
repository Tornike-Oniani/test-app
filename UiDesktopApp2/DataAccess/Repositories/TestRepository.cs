using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Migrations;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.DataAccess.Repositories
{
    public class TestRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<List<TestDTO>> GetAllTestsAsync()
        {
            var tests = await context.Tests
                .Include(t => t.ImageSets)
                    .ThenInclude(s => s.ImageVariants)
            .ToListAsync();

            foreach (Test test in tests)
            {
                test.ImageSets = test.ImageSets.OrderBy(ims => ims.Number).ToList();
            }

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

        public async Task DeleteTestById(int id)
        {
            Test? test = await context.Tests.FindAsync(id);
            if (test != null)
            {
                context.Tests.Remove(test);
                await context.SaveChangesAsync();
            }
        }
    }
}
