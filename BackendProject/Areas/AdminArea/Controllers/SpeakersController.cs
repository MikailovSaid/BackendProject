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
    public class SpeakersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SpeakersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Speakers> speakers = await _context.Speakers.ToListAsync();
            return View(speakers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var speakers = await GetSpeakerById(id);
            if (speakers is null) return NotFound();
            return View(speakers);
        }

        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Speakers speakers)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            if (!speakers.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }

            if (!speakers.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + speakers.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/event", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await speakers.Photo.CopyToAsync(stream);
            }
            speakers.Image = fileName;
            await _context.AddAsync(speakers);
            await _context.SaveChangesAsync();
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Speakers speakers = await GetSpeakerById(id);

            if (speakers == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/event", speakers.Image);

            Helper.DeleteFile(path);

            _context.Speakers.Remove(speakers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var speakers = await GetSpeakerById(id);
            if (speakers is null) return NotFound();
            
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return View(speakers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Speakers speakers)
        {
            ViewBag.events = await GetEventsBySpeakers();
            if (!ModelState.IsValid) return View(speakers);
            Speakers speaker = await _context.Speakers
                .Include(m => m.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speaker is null) return NotFound();



            if (speakers.Photo != null)
            {
                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/event", speaker.Image);
                Helper.DeleteFile(path);


                string fileName = Guid.NewGuid().ToString() + "_" + speakers.Photo.FileName;

                string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/event", fileName);

                await speakers.Photo.SaveFile(newPath);

                speakers.Image = fileName;
            }

            speaker.Name = speakers.Name;
            speaker.Profession = speakers.Profession;
            speaker.EventId = speakers.EventId;

            await _context.SaveChangesAsync();
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        private async Task<Speakers> GetSpeakerById(int id)
        {
            return await _context.Speakers.FindAsync(id);
        }

        private async Task<SelectList> GetEventsBySpeakers()
        {
            var events = await _context.Events.ToListAsync();
            return new SelectList(events, "Id", "Name");
        }
    }
}
