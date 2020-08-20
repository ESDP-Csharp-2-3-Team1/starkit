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
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class SuperAdminController : Controller
    {
        private StarkitContext _db { get; set; }

        private UserManager<User> _userManager { get; set; }

        public SuperAdminController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index( int page = 1)
        {
            string userId = _userManager.GetUserId(User);
           int pageSize = 5; 
           List<User> users = _db.Users.Where(u=>u.Id != userId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
           for (int i = 0; i < users.Count; i++)
           {
               users[i].PostalAddress = await _db.PostalAddresses.FirstOrDefaultAsync(p => p.UserId == users[i].Id);
               users[i].LegalAddress = await _db.LegalAddresses.FirstOrDefaultAsync(u => u.UserId == users[i].Id);
           }
           SuperAdminIndexPageInfo pageInfo = new SuperAdminIndexPageInfo { PageNumber=page, PageSize=pageSize, TotalItems=_db.Users.Count()};
           SuperAdminIndexViewModel ivm = new SuperAdminIndexViewModel { PageInfo = pageInfo, Users = users };
           
           return View(ivm);
                  
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateRegistrantStatus(string userId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.Status = user.Status == UserStatus.Unlocked ?  UserStatus.Locked : UserStatus.Unlocked;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return Json(Convert.ToString(user.Status)?.ToLower());
            }
            return Json(false);
        }
    }
}