using BackendProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<ServiceArea> ServiceAreas { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<AboutArea> AboutAreas { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseFeature> CourseFeatures { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
