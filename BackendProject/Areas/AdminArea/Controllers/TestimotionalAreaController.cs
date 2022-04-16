using BackendProject.Data;
using BackendProject.Models;
using BackendProject.Utilities.File;
using BackendProject.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class TestimotionalAreaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TestimotionalAreaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            TestimotionalArea testimotionalArea = await _context.TestimotionalAreas.FirstOrDefaultAsync();
            return View(testimotionalArea);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var testimotional = await GetTestimotionalAreaById(id);
            if (testimotional is null) return NotFound();
            return View(testimotional);
        }

        public async Task<IActionResult> Edit(int id)
        {
            TestimotionalArea testimotional = await GetTestimotionalAreaById(id);

            if (testimotional == null) return NotFound();

            return View(testimotional);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TestimotionalArea testimotional)
        {
            var dbTestimotional = await GetTestimotionalAreaById(id);
            if (dbTestimotional == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View(dbTestimotional);

            if (!testimotional.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbTestimotional);
            }

            if (!testimotional.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbTestimotional);
            }

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/testimonial", dbTestimotional.Image);

            Helper.DeleteFile(path);


            string fileName = Guid.NewGuid().ToString() + "_" + testimotional.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/testimonial", fileName);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await testimotional.Photo.CopyToAsync(stream);
            }

            dbTestimotional.Image = fileName;
            dbTestimotional.Name = testimotional.Name;
            dbTestimotional.Position = testimotional.Position;
            dbTestimotional.Desc = testimotional.Desc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private async Task<TestimotionalArea> GetTestimotionalAreaById(int id)
        {
            return await _context.TestimotionalAreas.FindAsync(id);
        }
    }
}
