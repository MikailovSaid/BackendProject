using BackendProject.Data;
using BackendProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class VideoTourController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public VideoTourController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            VideoTour videoTour = await _context.VideoTours.FirstOrDefaultAsync();
            return View(videoTour);
        }

        public async Task<IActionResult> Edit(int id)
        {
            VideoTour videoTour = await _context.VideoTours.Where(m=>m.Id == id).FirstOrDefaultAsync();

            if (videoTour == null) return NotFound();

            return View(videoTour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoTour videoTour)
        {
            if (!ModelState.IsValid) return View();

            if (id != videoTour.Id) return BadRequest();

            try
            {
                VideoTour dbVideoTour = await _context.VideoTours.FirstOrDefaultAsync();

                dbVideoTour.VideoLink = videoTour.VideoLink;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }
    }
}
