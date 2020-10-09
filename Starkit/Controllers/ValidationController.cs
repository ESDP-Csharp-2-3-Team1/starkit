using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class ValidationController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IRecaptchaService _recaptcha;

        public ValidationController(StarkitContext db, UserManager<User> userManager, SignInManager<User> signInManager, IRecaptchaService recaptcha)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _recaptcha = recaptcha;
        }
        // GET
        public async Task<bool> CheckEmail(string email)
        {
            return !await _db.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> VerifyingEmailAuthenticity(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public bool CheckIIN(string IIN)
        {
            string pattern = "^[0-9]+$";
            if (!Regex.IsMatch(IIN, pattern)) return false;
            return true;
        }

        public async Task<bool> CheckNameCategory(string name, string id)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.Users.
                    FirstOrDefaultAsync(u => u.Id == user.IdOfTheSelectedRestaurateur);
            
            if (id == null)
                return !_db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                                && c.RestaurantId == user.RestaurantId);
            
            List<Category> categories = _db.Categories.Where(c => c.Id != id && 
                                                                  c.RestaurantId == user.RestaurantId).ToList();
            return !categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        
        public async Task<bool> CheckNameDish(string name, string id)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.Users.
                    FirstOrDefaultAsync(u => u.Id == user.IdOfTheSelectedRestaurateur);
            
            if (id == null)
                return !_db.Dishes.Any(d => d.Name.ToLower().Trim() == name.ToLower().Trim() 
                                            && d.RestaurantId == user.RestaurantId);
            
            List<Dish> dishes = _db.Dishes.Where(d => d.Id != id && 
                                                                  d.RestaurantId == user.RestaurantId).ToList();
            return !dishes.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        
        public async Task<bool> CheckNameMenu(string name, string id)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.Users.
                    FirstOrDefaultAsync(u => u.Id == user.IdOfTheSelectedRestaurateur);
            
            if (id == null)
                return !_db.Menu.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                            && c.RestaurantId == user.RestaurantId);
            
            List<Menu> menu = _db.Menu.Where(d => d.Id != id && 
                                                      d.RestaurantId == user.RestaurantId).ToList();
            return !menu.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> CheckNameSubCategory(string name, string id)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.Users.
                    FirstOrDefaultAsync(u => u.Id == user.IdOfTheSelectedRestaurateur);
            
            if (id == null)
                return !_db.SubCategories.Any(sb => sb.Name.ToLower().Trim() == name.ToLower().Trim() 
                                                && sb.RestaurantId == user.RestaurantId);
            
            List<SubCategory> subCategories = _db.SubCategories.Where(sb => sb.Id != id && 
                                                                            sb.RestaurantId == user.RestaurantId).ToList();
            return !subCategories.Any(sb => sb.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        
        public async Task<bool> CheckNameStock(string name, string id)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.Users.
                    FirstOrDefaultAsync(u => u.Id == user.IdOfTheSelectedRestaurateur);
            
            string[] separator = {"+", "="};
            string[] arrWord = name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string name2 = $"{arrWord[1]}+{arrWord[0]}={arrWord[2]}";
            if (id == null)
            {
                return !_db.Stocks.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                            || c.Name.ToLower().Trim() == name2.ToLower().Trim()
                                            && c.RestaurantId == user.RestaurantId);   
            }
            List<Stock> stocks = _db.Stocks.Where(c => c.Id != id && 
                                                       c.RestaurantId == user.RestaurantId).ToList();
            return !stocks.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                    || c.Name.ToLower().Trim() == name2.ToLower().Trim());
        }
        
        public async Task<bool>CheckOldPassword(string oldPassword)
        {
            User user = _userManager.GetUserAsync(User).Result;
            var result = await _userManager.CheckPasswordAsync(user, oldPassword);
            return result;
        }
        
        public bool ComparePasswords(string oldPassword, string newPassword)
        {
            return oldPassword != newPassword;
        }

        public bool CheckTableCapacity(int tableId, int pax)
        {
            Table table = _db.Tables.FirstOrDefault(t => t.Id == tableId);
            if (table.Capacity < pax)
                return false;
            return true;
        }

        public bool CheckTime(string bookFrom, string bookTo)
        {
            var timeFrom = Convert.ToInt32(bookFrom.Split(":")[0]);
            var timeTo = Convert.ToInt32(bookTo.Split(":")[0]);
            if (timeFrom > timeTo)
            {
                return false;
            }
            return timeFrom + 1 <= timeTo;
        }
        public async Task<bool> CheckDomain(string domainName)
        {
            return !await _db.Restaurants.AnyAsync(r => r.DomainName == domainName);
        }
    }
}