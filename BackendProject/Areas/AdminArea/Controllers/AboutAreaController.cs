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
    public class AboutAreaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutAreaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            AboutArea aboutAreas = await _context.AboutAreas.AsNoTracking().FirstOrDefaultAsync();
            return View(aboutAreas);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var aboutAreas = await GetAboutAreaById(id);
            if (aboutAreas is null) return NotFound();
            return View(aboutAreas);
        }

        public async Task<IActionResult> Edit(int id)
        {
            AboutArea aboutArea = await GetAboutAreaById(id);

            if (aboutArea == null) return NotFound();

            return View(aboutArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutArea aboutArea)
        {
            var dbAboutArea = await GetAboutAreaById(id);
            if (dbAboutArea == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View(dbAboutArea);

            if (!aboutArea.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbAboutArea);
            }

            if (!aboutArea.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbAboutArea);
            }

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", dbAboutArea.Image);

            Helper.DeleteFile(path);


            string fileName = Guid.NewGuid().ToString() + "_" + aboutArea.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", fileName);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await aboutArea.Photo.CopyToAsync(stream);
            }

            dbAboutArea.Image = fileName;
            dbAboutArea.Header = aboutArea.Header;
            dbAboutArea.Text = aboutArea.Text;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private async Task<AboutArea> GetAboutAreaById(int id)
        {
            return await _context.AboutAreas.FindAsync(id);
        }
    }
}
