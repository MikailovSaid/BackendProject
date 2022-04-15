using BackendProject.Data;
using BackendProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class NoticeBoardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public NoticeBoardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<NoticeBoard> noticeBoards = await _context.NoticeBoards.ToListAsync();
            return View(noticeBoards);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var notice = await GetNoticeById(id);
            if (notice is null) return NotFound();
            return View(notice);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticeBoard notice)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            await _context.NoticeBoards.AddAsync(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var notice = await GetNoticeById(id);

            if (notice == null) return NotFound();

            _context.NoticeBoards.Remove(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            NoticeBoard notice = await _context.NoticeBoards.Where(m=>m.Id == id).FirstOrDefaultAsync();

            if (notice == null) return NotFound();

            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoticeBoard notice)
        {
            if (!ModelState.IsValid) return View();

            if (id != notice.Id) return BadRequest();

            try
            {
                NoticeBoard dbNotice = await _context.NoticeBoards.Where(m=>m.Id == id).FirstOrDefaultAsync();

                dbNotice.Date = notice.Date;
                dbNotice.Desc = notice.Desc;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        private async Task<NoticeBoard> GetNoticeById(int id)
        {
            return await _context.NoticeBoards.FindAsync(id);
        }
    }
}
