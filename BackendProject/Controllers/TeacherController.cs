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
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TeacherDetail(int id)
        {
            Teacher teacher = await _context.Teachers.Where(m => m.Id == id).FirstOrDefaultAsync();
            TeacherContact teacherContact = await _context.TeacherContacts.Where(m => m.TeacherId == teacher.Id).FirstOrDefaultAsync();
            TeacherSkills teacherSkills = await _context.TeacherSkills.Where(m => m.TeacherId == teacher.Id).FirstOrDefaultAsync();

            TeacherVM teacherVM = new TeacherVM()
            {
                Teacher = teacher,
                TeacherContact = teacherContact,
                TeacherSkills = teacherSkills
            };
            return View(teacherVM);
        }
    }
}
