using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class MenuController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public MenuController(StarkitContext db, IHostEnvironment environment, UploadService uploadService, UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        private string Load(string id, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Menu\\{id}");
            string photoPath = $"images/{userId}/Menu/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/{userId}/Menu/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/{userId}/Menu/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        private void DeleteMenuAvatar(Menu menu)
        {
            string userId = _userManager.GetUserId(User);
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Menu\\" + menu.Id; 
            if (Directory.Exists(filePath))
            {
                System.IO.File.Delete("wwwroot/" + menu.Avatar);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}