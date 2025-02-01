using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Services
{
    public class TestRepository(ApplicationDbContext context)
    {
        public Task CreateTest(TestDTO test)
        {
            context.Tests.Add(new Test()
            {
                Name = test.Name,
            });

            return context.SaveChangesAsync();
        }
    }
}
