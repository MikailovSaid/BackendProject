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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await GetCategoryById(id);
            if (category is null) return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            bool isExist = _context.Categories.Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await GetCategoryById(id);

            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (!ModelState.IsValid) return View();

            if (id != category.Id) return BadRequest();

            try
            {
                Category dbCategory = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                bool isExist = _context.Categories.Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());
                if (isExist)
                {
                    ModelState.AddModelError("Name", "This category already exists");
                    return View();
                }

                dbCategory.Name = category.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        private async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
