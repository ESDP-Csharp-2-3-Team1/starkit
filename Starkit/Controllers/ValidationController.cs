using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public bool CheckNameCategory(string name, string id)
        {
            if (id == null)
                return !_db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                                && c.UserId == _userManager.GetUserId(User));
            
            List<Category> categories = _db.Categories.Where(c => c.Id != id && 
                                                                  c.UserId == _userManager.GetUserId(User)).ToList();
            return !categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        
        public bool CheckNameDish(string name, string id)
        {
            if (id == null)
                return !_db.Dishes.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                                && c.CreatorId == _userManager.GetUserId(User));
            
            List<Dish> dishes = _db.Dishes.Where(d => d.Id != id && 
                                                                  d.CreatorId == _userManager.GetUserId(User)).ToList();
            return !dishes.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        
        public bool CheckNameMenu(string name, string id)
        {
            if (id == null)
                return !_db.Menu.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                            && c.CreatorId == _userManager.GetUserId(User));
            
            List<Menu> menu = _db.Menu.Where(d => d.Id != id && 
                                                      d.CreatorId == _userManager.GetUserId(User)).ToList();
            return !menu.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool CheckNameSubCategory(string name)
        {
            List<Category> categories = _db.Categories.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
            return !_db.Categories.Any(c => c.UserId == _userManager.GetUserId(User) &&
                                           c.SubCategories.Any(s => s.Name.ToLower().Trim() == name.ToLower().Trim()));
        }
        
        public bool CheckNameStock(string name, string id)
        {
            string[] separator = {"+", "="};
            string[] arrWord = name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string name2 = $"{arrWord[1]}+{arrWord[0]}={arrWord[2]}";
            if (id == null)
            {
                return !_db.Stocks.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim() 
                                            || c.Name.ToLower().Trim() == name2.ToLower().Trim()
                                            && c.CreatorId == _userManager.GetUserId(User));   
            }

            List<Stock> stocks = _db.Stocks.Where(c => c.Id != id && 
                                                       c.CreatorId == _userManager.GetUserId(User)).ToList();
            return !stocks.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
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
    }
}