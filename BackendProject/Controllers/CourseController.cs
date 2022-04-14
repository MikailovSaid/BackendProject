using BackendProject.Data;
using BackendProject.Models;
using BackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId)
        {
            return View(categoryId);
        }
        public async Task<IActionResult> CourseDetail(int id)
        {
            Course courses = await _context.Courses.Where(m=>m.Id == id).Include(m=>m.CourseFeature).FirstOrDefaultAsync();
            CourseFeature courseFeatures = await _context.CourseFeatures.Where(m => m.CourseId == id).FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.ToListAsync();

            CourseVM courseVM = new CourseVM()
            {
                Courses = courses,
                CourseFeatures = courseFeatures,
                Categories = categories
            };

            return View(courseVM);
        }
    }
}
