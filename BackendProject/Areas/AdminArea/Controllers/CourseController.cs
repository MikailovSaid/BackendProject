using BackendProject.Data;
using BackendProject.Models;
using BackendProject.Utilities.File;
using BackendProject.Utilities.Helpers;
using BackendProject.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _context.Courses.ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var course = await GetCourseById(id);
            if (course is null) return NotFound();
            return View(course);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseVM courseVM)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            if (!courseVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photos", "Image type is wrong");
                return View();
            }

            if (!courseVM.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photos", "Image size is wrong");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/course", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await courseVM.Photo.CopyToAsync(stream);
            }

            CourseFeature courseFeature = new CourseFeature
            {
                StartDate = courseVM.StartDate,
                Duration = courseVM.Duration,
                ClassDuration = courseVM.ClassDuration,
                SkillLevel = courseVM.SkillLevel,
                Language = courseVM.Language,
                StudentsCount = courseVM.StudentsCount,
                Assesments = courseVM.Assesments,
                Price = courseVM.Price,
            };
            Course course = new Course
            {
                Image = fileName,
                Name = courseVM.Name,
                Desc = courseVM.Desc,
                AboutCourse = courseVM.AboutCourse,
                AboutDesc = courseVM.AboutDesc,
                ApplyCourse = courseVM.ApplyCourse,
                ApplyDesc = courseVM.ApplyDesc,
                CertificationCourse = courseVM.CertificationCourse,
                CertificationDesc = courseVM.CertificationDesc,
                CourseFeature = courseFeature,
                CategoryId = courseVM.CategoryId
            };

            await _context.AddAsync(course);
            await _context.SaveChangesAsync();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Course course = await GetCourseById(id);

            if (course == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/course", course.Image);

            Helper.DeleteFile(path);

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await GetCourseById(id);
            if (course is null) return NotFound();
            var courseFeature = await _context.CourseFeatures.FirstOrDefaultAsync(m => m.CourseId == id);
            CourseVM courseVM = new CourseVM
            {
                //Id = course.Id,
                Name = course.Name,
                AboutCourse = course.AboutCourse,
                AboutDesc = course.AboutDesc,
                ApplyCourse = course.ApplyCourse,
                ApplyDesc = course.ApplyDesc,
                CertificationCourse = course.CertificationCourse,
                CertificationDesc = course.CertificationDesc,
                CategoryId = course.CategoryId,
                StartDate = courseFeature.StartDate,
                Duration = courseFeature.Duration,
                ClassDuration =  courseFeature.ClassDuration,
                SkillLevel = courseFeature.SkillLevel,
                Language = courseFeature.Language,
                StudentsCount = courseFeature.StudentsCount,
                Assesments = courseFeature.Assesments,
                Price = courseFeature.Price
            };
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(courseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseVM courseVM)
        {
            ViewBag.categories = await GetCategoriesByCourses();
            if (!ModelState.IsValid) return View(courseVM);
            Course course = await _context.Courses
                .Include(m => m.Category)
                .Include(m => m.CourseFeature)
                .FirstOrDefaultAsync(m=>m.Id == id);
            if (course is null) return NotFound();



            if (courseVM.Photo != null)
            {
                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/course", course.Image);
                Helper.DeleteFile(path);


                string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photo.FileName;

                string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/course", fileName);

                await courseVM.Photo.SaveFile(newPath);

                course.Image = fileName;
            }

            course.Name = courseVM.Name;
            course.AboutCourse = courseVM.AboutCourse;
            course.AboutDesc = courseVM.AboutDesc;
            course.ApplyCourse = courseVM.ApplyCourse;
            course.ApplyDesc = courseVM.ApplyDesc;
            course.CertificationCourse = courseVM.CertificationCourse;
            course.CertificationDesc = courseVM.CertificationDesc;
            course.CategoryId = courseVM.CategoryId;

            course.CourseFeature.StartDate = courseVM.StartDate;
            course.CourseFeature.Duration = courseVM.Duration;
            course.CourseFeature.ClassDuration = courseVM.ClassDuration;
            course.CourseFeature.SkillLevel = courseVM.SkillLevel;
            course.CourseFeature.Language = courseVM.Language;
            course.CourseFeature.StudentsCount = courseVM.StudentsCount;
            course.CourseFeature.Assesments = courseVM.Assesments;
            course.CourseFeature.Price = courseVM.Price;

            await _context.SaveChangesAsync();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        private async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }
        private async Task<SelectList> GetCategoriesByCourses()
        {
            var categories = await _context.Categories.ToListAsync();
            return new SelectList(categories, "Id", "Name");
        }
    }
}
