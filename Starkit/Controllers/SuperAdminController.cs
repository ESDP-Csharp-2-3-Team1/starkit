using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class SuperAdminController : Controller
    {
        public StarkitContext _db { get; set; }

        public UserManager<User> _userManager { get; set; }

        public SuperAdminController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            List<User> users = _db.Users.Where(u=>u.Id != _userManager.GetUserId(User)).ToList();
            return View(users);
        }
    }
}