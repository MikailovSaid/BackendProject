using BackendProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewComponents
{
    public class SubscribeAreaViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public SubscribeAreaViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var subscribearea = await _context.SubscribeAreas.FirstOrDefaultAsync();
            return (await Task.FromResult(View(subscribearea)));
        }
    }
}
