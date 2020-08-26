using System;
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
    public class EmployeesController : Controller
    {
        private UserManager<User> _userManager { get; set; }
        private StarkitContext _db { get; set; }

        public EmployeesController(UserManager<User> userManager, StarkitContext db)
        {
            _userManager = userManager;
            _db = db;
        }


        // GET
        [Authorize]
        public async Task<IActionResult> Create()
        {
            string userId = _userManager.GetUserId(User);
            if (!await _db.Users.AnyAsync(u => u.Id == userId && u.RestaurantId != null))
                return RedirectToAction("Register", "Restaurants");
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (!await _db.Users.AnyAsync(u => u.Id == userId && u.RestaurantId != null))
                    return RedirectToAction("Register", "Restaurants");
                
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    User newUser = new User
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        Name = model.Name,
                        SurName = model.Surname,
                        Position = model.Position,
                        RestaurantId = user.RestaurantId
                    };
                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, Convert.ToString(newUser.Position));
                        return RedirectToAction("Index", "Users");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}