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
using BackendProject.Utilities.File;
using BackendProject.Utilities.Helpers;
using System.IO;

namespace BackendProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var blog = await GetBlogById(id);
            if (blog is null) return NotFound();
            return View(blog);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {

            if (ModelState.ValidationState == ModelValidationState.Invalid) return View();

            if (!blog.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photos", "Image type is wrong");
                return View();
            }

            if (!blog.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photos", "Image size is wrong");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await blog.Photo.CopyToAsync(stream);
            }

            blog.Image = fileName;
            await _context.Blogs.AddAsync(blog);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await GetBlogById(id);

            if (blog == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", blog.Image);

            Helper.DeleteFile(path);

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blog = await GetBlogById(id);
            if (blog is null) return NotFound();
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
            var dbBlog = await GetBlogById(id);
            if (dbBlog == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View(dbBlog);

            if (!blog.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbBlog);
            }

            if (!blog.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbBlog);
            }

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", dbBlog.Image);

            Helper.DeleteFile(path);


            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await blog.Photo.CopyToAsync(stream);
            }

            dbBlog.Image = fileName;
            dbBlog.Text = blog.Text;
            dbBlog.Desc = blog.Desc;
            dbBlog.Author = blog.Author;
            dbBlog.Date = blog.Date;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<Blog> GetBlogById(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
    }
}
