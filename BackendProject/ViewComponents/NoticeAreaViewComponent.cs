using BackendProject.Data;
using BackendProject.Models;
using BackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewComponents
{
    public class NoticeAreaViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public NoticeAreaViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<NoticeBoard> noticeBoards = await _context.NoticeBoards.ToListAsync();
            VideoTour videoTour = await _context.VideoTours.FirstOrDefaultAsync();

            NoticeAreaVM noticeAreaVM = new NoticeAreaVM()
            {
                NoticeBoards = noticeBoards,
                VideoTour = videoTour
            };

            return (await Task.FromResult(View(noticeAreaVM)));
        }
    }
}
