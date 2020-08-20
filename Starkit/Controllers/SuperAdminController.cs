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
        
        [Authorize(Roles = "SuperAdmin")]
        [ActionName("Index")]
        [HttpPost]
        public async Task<IActionResult> EditRegistrantInfo(SuperAdminIndexViewModel model)
        {
            // if (ModelState.IsValid)
            // {
                // _db.Users.Update(model.User);
                User user = await BuildUser(model.User);
                LegalAddress legalAddress = await BuildLegalAddress(model.User.LegalAddress, model.User.Id);
                PostalAddress postalAddress = await BuildPostalAddress(model.User.PostalAddress, model.User.Id);
                _db.LegalAddresses.Update(legalAddress);
                _db.PostalAddresses.Update(postalAddress);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            // }
            // return NotFound();
        }

        [NonAction]
        [Authorize(Roles = "SuperAdmin")]
        private async Task<PostalAddress> BuildPostalAddress(PostalAddress model, string userId)
        {
            PostalAddress postalAddress = await _db.PostalAddresses.FirstOrDefaultAsync(l => l.UserId == userId);
            postalAddress.Index = model.Index;
            postalAddress.Country = model.Country;
            postalAddress.Region = model.Region;
            postalAddress.City = model.City;
            postalAddress.Address = model.Address;
            return postalAddress;
        }


        [NonAction]
        [Authorize(Roles = "SuperAdmin")]
        private async Task<LegalAddress> BuildLegalAddress(LegalAddress model, string userId)
        {
            LegalAddress legalAddress = await _db.LegalAddresses.FirstOrDefaultAsync(l => l.UserId == userId);
            legalAddress.Index = model.Index;
            legalAddress.Country = model.Country;
            legalAddress.Region = model.Region;
            legalAddress.City = model.City;
            legalAddress.Address = model.Address;
            return legalAddress;
        }

        [NonAction]
        [Authorize(Roles = "SuperAdmin")]
        private async Task<User> BuildUser(User model)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
            user.Name = model.Name;
            user.SurName = model.SurName;
            user.IIN = model.IIN;
            user.CompanyName = model.CompanyName;
            user.Email = model.Email;
            user.CityPhone = model.CityPhone;
            user.PhoneNumber = model.PhoneNumber;
            return user;
        }
    }
}