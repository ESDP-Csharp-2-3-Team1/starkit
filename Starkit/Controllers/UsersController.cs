using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class UsersController : Controller
    {
        public UserManager<User> _userManager { get; set; }
        public StarkitContext _db { get; set; }

        public UsersController(UserManager<User> userManager, StarkitContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
                    
            string userId = _userManager.GetUserId(User);
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

        [Authorize]
        public async Task<IActionResult> UpdateUserInformation(EditUserViewModel model)
        {
            if (model != null)
            {
                string userId = _userManager.GetUserId(User);
                LegalAddress legalAddress = await _db.LegalAddresses.FirstOrDefaultAsync(l => l.UserId == userId);
                PostalAddress postalAddress = await _db.PostalAddresses.FirstOrDefaultAsync(p => p.UserId == userId);
                if (legalAddress is null || postalAddress is null)
                    return NotFound();
                legalAddress.City = model.LegalAddress.City;
                legalAddress.Region = model.LegalAddress.Region;
                postalAddress.City = model.PostalAddress.City;
                postalAddress.Region = model.PostalAddress.Region;
                if (!string.IsNullOrEmpty(model.CityPhone))
                {
                    User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    user.CityPhone = model.CityPhone;
                    user.CityPhone = model.CityPhone;
                    _db.Users.Update(user);
                }
                _db.LegalAddresses.Update(legalAddress);
                _db.PostalAddresses.Update(postalAddress);
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}