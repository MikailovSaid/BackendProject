using BackendProject.Data;
using BackendProject.Models;
using BackendProject.Utilities.Helpers;
using BackendProject.ViewModels.Admin;
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
    public class ServiceAreaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ServiceAreaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<ServiceArea> serviceAreas = await _context.ServiceAreas.AsNoTracking().ToListAsync();
            return View(serviceAreas);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var serviceAreas = await GetServiceAreaById(id);
            if (serviceAreas is null) return NotFound();
            return View(serviceAreas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceArea serviceArea)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            await _context.ServiceAreas.AddAsync(serviceArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceArea serviceArea = await GetServiceAreaById(id);

            if (serviceArea == null) return NotFound();

            _context.ServiceAreas.Remove(serviceArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ServiceArea serviceArea = await _context.ServiceAreas.FirstOrDefaultAsync();
            
            if (serviceArea == null) return NotFound();

            return View(serviceArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceArea serviceArea)
        {
            if (!ModelState.IsValid) return View();

            if (id != serviceArea.Id) return BadRequest();

            try
            {
                ServiceArea dbServiceArea = await _context.ServiceAreas.FirstOrDefaultAsync();

                dbServiceArea.Header = serviceArea.Header;
                dbServiceArea.Description = serviceArea.Description;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        private async Task<ServiceArea> GetServiceAreaById(int id)
        {
            return await _context.ServiceAreas.FindAsync(id);
        }
    }
}
