﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess.Entities;

namespace UiDesktopApp2.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=results.db");
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<ImageSet> ImageSets { get; set; }
        public DbSet<ImageVariant> ImageVariants { get; set; }
    }
}
