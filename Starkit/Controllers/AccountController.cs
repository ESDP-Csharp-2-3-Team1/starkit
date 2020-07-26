using System;
using System.Runtime.CompilerServices;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        model.RememberMe,
                        false
                        );
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Starkit");
                    ModelState.AddModelError("","Неверный пароль пользователя");
                }
                else
                    ModelState.AddModelError("","E-mail не зарегистрирован.");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if(ModelState.IsValid)
            {
                User newUser = new User()
                {
                    UserName =  model.Email,
                    Name = model.Name,
                    SurName = model.SurName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CityPhone = model.CityPhone,
                    CompanyName = model.CompanyName,
                    IIN = model.IIN
                };
                model.PostalAddress.UserId = newUser.Id;
                model.LegalAddress.UserId = newUser.Id;
                
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "restaurateur");
                    await _signInManager.SignInAsync(newUser, false);
                    await _db.LegalAddresses.AddAsync(model.LegalAddress);
                    await _db.PostalAddresses.AddAsync(model.PostalAddress);
                    return RedirectToAction("Index", "Starkit");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(String.Empty, error.Description);
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

       
    }
}