using BackendProject.Data;
using BackendProject.Models;
using BackendProject.Utilities.File;
using BackendProject.Utilities.Helpers;
using BackendProject.ViewModels.Admin;
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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Sliders> sliders = await _context.Sliders.AsNoTracking().ToListAsync();
            return View(sliders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            foreach (var photo in sliderVM.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "Image type is wrong");
                    return View();
                }

                if (!photo.CheckFileSize(1800))
                {
                    ModelState.AddModelError("Photos", "Image size is wrong");
                    return View();
                }

            }

            foreach (var photo in sliderVM.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }


                Sliders slider = new Sliders
                {
                    Image = fileName,
                    Header = sliderVM.Header,
                    Description = sliderVM.Description
                };

                await _context.Sliders.AddAsync(slider);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Sliders slider = await GetSliderById(id);

            if (slider == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", slider.Image);

            Helper.DeleteFile(path);

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sliders slider)
        {
            var dbSlider = await GetSliderById(id);
            if (dbSlider == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View(dbSlider);

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbSlider);
            }

            if (!slider.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbSlider);
            }

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", dbSlider.Image);

            Helper.DeleteFile(path);


            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", fileName);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            dbSlider.Image = fileName;
            dbSlider.Header = slider.Header;
            dbSlider.Description = slider.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<Sliders> GetSliderById(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }
    }
}
