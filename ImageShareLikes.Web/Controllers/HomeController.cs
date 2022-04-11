using ImageShareLikes.Data;
using ImageShareLikes.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageShareLikes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        private IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var repo = new ImageRepository(_configuration.GetConnectionString("ConStr"));
            var vm = new HomePageViewModel
            {
                Images = repo.GetImages()
            };
            return View(vm);
        }

        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRepository(_configuration.GetConnectionString("ConStr"));
            var ids = HttpContext.Session.Get<List<int>>("ids");
            var vm = new ViewImageViewModel
            {
                Image = repo.GetById(id),
                DisableLike =  ids != null && ids.Contains(id)
            };
            return View(vm);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            imageFile.CopyTo(stream);
            image.FileName = fileName;
            var repo = new ImageRepository(_configuration.GetConnectionString("ConStr"));
            repo.AddImage(image);
            return RedirectToAction("Index");
        }

        [HttpPost]

        public void LikeImage(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids");
            if(ids == null)
            {
                ids = new List<int>();
            }
            ids.Add(id);
            HttpContext.Session.Set("ids", ids);
            var repo = new ImageRepository(_configuration.GetConnectionString("ConStr"));
            repo.IncrementImageLikes(id);
        }

        public IActionResult GetIamgeLikes(int id)
        {
            var repo = new ImageRepository(_configuration.GetConnectionString("ConStr"));
            return Json(repo.GetIamgeLikes(id));
        }
    }
}
