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
    public class TestimotionalAreaViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public TestimotionalAreaViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            TestimotionalArea testimotionalArea = await _context.TestimotionalAreas.FirstOrDefaultAsync();

            return (await Task.FromResult(View(testimotionalArea)));
        }
    }
}
