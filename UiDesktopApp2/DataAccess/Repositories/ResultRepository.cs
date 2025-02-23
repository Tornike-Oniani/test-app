using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
