using System;
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
    public class EmployeesController : Controller
    {
        private UserManager<User> _userManager { get; set; }
        private StarkitContext _db { get; set; }
        
        
        // GET
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                User newUser = new User {Email = model.Email,Name = model.Name, SurName = model.Surname, Position = model.Position, RestaurantId = user.RestaurantId};
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded) return RedirectToAction("Index", "Users");
                foreach (var error in result.Errors) ModelState.AddModelError(String.Empty, error.Description);
            }
            return View(model);
        }
    }
}