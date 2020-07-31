using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;

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
            ViewBag.User = await _userManager.FindByIdAsync(userId);
            return View();
        }
    }
}