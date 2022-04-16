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
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _context.Events.Skip(4).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> EventDetail(int id)
        {
            Event events = await _context.Events.Where(m=>m.Id == id).FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Speakers> speakers = await _context.Speakers.Where(m=>m.EventId == events.Id).ToListAsync();

            EventVM eventVM = new EventVM()
            {
                Event = events,
                Categories = categories,
                Speakers = speakers
            };

            return View(eventVM);
        }
    }
}
