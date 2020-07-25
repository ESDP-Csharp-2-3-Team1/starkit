using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<User> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public SignInManager<User> _signInManager { get; set; }
        public StarkitContext _db { get; set; }

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, StarkitContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
        }
        
        // GET

        public IActionResult Login()
        {
            return null;
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Register(Register model)
        {
            if(ModelState.IsValid)
            {
                
            }
            return View(model);
        }
       
    }
}