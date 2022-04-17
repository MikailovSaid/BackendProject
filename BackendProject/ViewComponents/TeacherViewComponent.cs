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
    public class TeacherViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public TeacherViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var newTake = take ?? _context.Courses.Count();

            List<Teacher> teachers = await _context.Teachers.Take(newTake).ToListAsync();
            return (await Task.FromResult(View(teachers)));
        }
    }
}
