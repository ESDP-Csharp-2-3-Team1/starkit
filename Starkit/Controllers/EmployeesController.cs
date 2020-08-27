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
        public async Task<IActionResult> Index( int page = 1)
        { 
            string userId = _userManager.GetUserId(User);
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            int pageSize = 5; 
            List<User> users = _db.Users.Where(u=>u.RestaurantId == user.RestaurantId && u.Position == EmployeePosition.AdministratorRestaurant || u.Position == EmployeePosition.ContentManager ).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            SuperAdminIndexPageInfo pageInfo = new SuperAdminIndexPageInfo { PageNumber=page, PageSize=pageSize, TotalItems = _db.Users.Count(u=>u.RestaurantId == user.RestaurantId && u.Position == EmployeePosition.AdministratorRestaurant || u.Position == EmployeePosition.ContentManager)};
            EmployeeIndexViewModel ivm = new EmployeeIndexViewModel{ PageInfo = pageInfo, Users = users };
            return View(ivm);
                  
        }
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _userManager.FindByIdAsync(userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
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
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _userManager.FindByIdAsync(userId);
                    userId = admin.IdOfTheSelectedRestaurateur;
                }
                
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
                        RestaurantId = user.RestaurantId,
                        CompanyName = user.CompanyName,
                        Position = model.Position switch
                        {
                            true => EmployeePosition.ContentManager,
                            false => EmployeePosition.AdministratorRestaurant
                        }
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

        [HttpPost]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid || model.ConfirmPassword == null && model.Password == null)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                user.Name = model.Name;
                user.SurName = model.Surname;
                user.Email = model.Email;
                user.Position = model.Position switch
                {
                    true => EmployeePosition.ContentManager,
                    false => EmployeePosition.AdministratorRestaurant
                };
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index","Employees");
            }
            return NotFound();
        }
    }
}