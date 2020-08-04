using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
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
    }
}