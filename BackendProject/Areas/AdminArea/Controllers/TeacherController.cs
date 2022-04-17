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
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TeacherController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = await _context.Teachers.ToListAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var teacher = await GetTeacherById(id);
            if (teacher is null) return NotFound();
            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherVM teacherVM)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            if (!teacherVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photos", "Image type is wrong");
                return View();
            }

            if (!teacherVM.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photos", "Image size is wrong");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + teacherVM.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/teacher", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await teacherVM.Photo.CopyToAsync(stream);
            }
            TeacherContact teacherContact = new TeacherContact
            {
                Email = teacherVM.Email,
                Phone = teacherVM.Phone,
                Skype = teacherVM.Skype
            };
            TeacherSkills teacherSkills = new TeacherSkills
            {
                Language = teacherVM.Language,
                TeamLeader = teacherVM.TeamLeader,
                Development = teacherVM.Development,
                Design = teacherVM.Design,
                Innovation = teacherVM.Innovation,
                Communication = teacherVM.Communication
            };
            Teacher teacher = new Teacher
            {
                Image = fileName,
                Name = teacherVM.Name,
                Profession = teacherVM.Profession,
                About = teacherVM.About,
                Degree = teacherVM.Degree,
                Experience = teacherVM.Experience,
                Hobbies = teacherVM.Hobbies,
                Faculty = teacherVM.Faculty,
                TeacherContact = teacherContact,
                TeacherSkills = teacherSkills,
            };

            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await GetTeacherById(id);

            if (teacher == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/teacher", teacher.Image);

            Helper.DeleteFile(path);

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await GetTeacherById(id);
            if (teacher is null) return NotFound();
            var teacherContact = await _context.TeacherContacts.FirstOrDefaultAsync(m => m.TeacherId == id);
            var teacherSkills = await _context.TeacherSkills.FirstOrDefaultAsync(m => m.TeacherId == id);

            TeacherVM teacherVM = new TeacherVM
            {
                Name = teacher.Name,
                Profession = teacher.Profession,
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Email = teacherContact.Email,
                Phone = teacherContact.Phone,
                Skype = teacherContact.Skype,
                Language = teacherSkills.Language,
                TeamLeader = teacherSkills.TeamLeader,
                Development = teacherSkills.Development,
                Design = teacherSkills.Design,
                Innovation = teacherSkills.Innovation,
                Communication = teacherSkills.Communication
            };
            return View(teacherVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeacherVM teacherVM)
        {
            if (!ModelState.IsValid) return View(teacherVM);
            Teacher teacher = await _context.Teachers
                .Include(m => m.TeacherContact)
                .Include(m => m.TeacherSkills)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher is null) return NotFound();



            if (teacherVM.Photo != null)
            {
                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/teacher", teacher.Image);
                Helper.DeleteFile(path);


                string fileName = Guid.NewGuid().ToString() + "_" + teacherVM.Photo.FileName;

                string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/teacher", fileName);

                await teacherVM.Photo.SaveFile(newPath);

                teacher.Image = fileName;
            }

            teacher.Name = teacherVM.Name;
            teacher.Profession = teacherVM.Profession;
            teacher.Degree = teacherVM.Degree;
            teacher.Experience = teacherVM.Experience;
            teacher.Hobbies = teacherVM.Hobbies;
            teacher.Faculty = teacherVM.Faculty;

            teacher.TeacherContact.Email = teacherVM.Email;
            teacher.TeacherContact.Phone = teacherVM.Phone;
            teacher.TeacherContact.Skype = teacherVM.Skype;

            teacher.TeacherSkills.Language = teacherVM.Language;
            teacher.TeacherSkills.TeamLeader = teacherVM.TeamLeader;
            teacher.TeacherSkills.Development = teacherVM.Development;
            teacher.TeacherSkills.Design = teacherVM.Design;
            teacher.TeacherSkills.Innovation = teacherVM.Innovation;
            teacher.TeacherSkills.Communication = teacherVM.Communication;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<Teacher> GetTeacherById(int id)
        {
            return await _context.Teachers.FindAsync(id);
        }
    }
}
