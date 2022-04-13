using BackendProject.Data;
using BackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public CourseViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? take, int? categoryId)
        {
            
            var newTake = take ?? _context.Courses.Count();
            List<Course> courses = null;
            if (categoryId is null || categoryId == 0)
            {
                 courses = await _context.Courses
                    .Include(m => m.CourseFeatures)
                    .Take(newTake)
                    .ToListAsync();
            }
            else
            {
                 courses = await _context.Courses
                    .Include(m => m.CourseFeatures)
                    .Where(m=>m.CategoryId == categoryId)
                    .Take(newTake)
                    .ToListAsync();
            }
            

            return (await Task.FromResult(View(courses)));
        }
    }
}
