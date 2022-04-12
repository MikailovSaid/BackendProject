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
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Sliders> sliders = await _context.Sliders.ToListAsync();
            List<ServiceArea> serviceAreas = await _context.ServiceAreas.ToListAsync();

            HomeVM homeVM = new HomeVM()
            {
                Sliders = sliders,
                ServiceAreas = serviceAreas
            };

            return View(homeVM);
        }
    }
}
