using BackendProject.Data;
using BackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewComponents
{
    public class AboutAreaViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public AboutAreaViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var aboutArea = await _context.AboutAreas.FirstOrDefaultAsync();

            return (await Task.FromResult(View(aboutArea)));
        }
    }
}
