using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.DataAccess.Repositories
{
    public class ResultRepository(ApplicationDbContext context, IMapper mapper)
    {
        public async Task<int> CreateResult(ResultDTO resultDTO)
        {
            var result = mapper.Map<Result>(resultDTO);
            context.Results.Add(result);
            await context.SaveChangesAsync();
            return result.Id;
        }

        public async Task<bool> HasTestAlreadyRun(int testId)
        {
            return await context.Results.AnyAsync(r => r.TestId == testId);
        }
    }
}
