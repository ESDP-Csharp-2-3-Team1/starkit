#nullable enable
using System;
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
    [Authorize(Roles = "SuperAdmin, Registrant")]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private StarkitContext _db;

        public UsersController(UserManager<User> userManager, StarkitContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(string cityphone,string postalCity, string postalRegion, string legalCity, string legalRegion, string apiKey)
        {
            string userId = _userManager.GetUserId(User);
                LegalAddress legalAddress = await _db.LegalAddresses.FirstOrDefaultAsync(l => l.UserId == userId);
                PostalAddress postalAddress = await _db.PostalAddresses.FirstOrDefaultAsync(p => p.UserId == userId);
              
                if (!(legalCity is null))
                    legalAddress.City = legalCity;
                if (!(legalRegion is null))
                    legalAddress.Region = legalRegion;
                if (!(postalCity is null))
                    postalAddress.City = postalRegion;
                if (!(postalCity is null))
                    postalAddress.Region = postalCity;
                if (!(cityphone is null))
                {
                    User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    user.CityPhone = cityphone;
                    _db.Users.Update(user);
                }
                if (!(apiKey is null))
                {
                    User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    user.GoogleMapsApi = apiKey;
                    _db.Users.Update(user);
                }
                _db.LegalAddresses.Update(legalAddress);
                _db.PostalAddresses.Update(postalAddress);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            EditUserViewModel model = new EditUserViewModel()
            {
                Id = userId,
                LegalAddress = _db.LegalAddresses.FirstOrDefault(l => l.UserId == userId),
                PostalAddress = _db.PostalAddresses.FirstOrDefault(p => p.UserId == userId)
            };
            ViewBag.LegalAddress = _db.LegalAddresses.FirstOrDefault(l => l.UserId == userId);
            ViewBag.PostalAddress = _db.PostalAddresses.FirstOrDefault(p => p.UserId == userId);
            ViewBag.User = await _userManager.FindByIdAsync(userId);
            return View(model);
        }
    }
}