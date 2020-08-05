using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class ValidationController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public ValidationController(StarkitContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET
        public async Task<bool> CheckEmail(string email)
        {
            return !await _db.Users.AnyAsync(u => u.Email == email);
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

        public bool CheckNameSubCategory(string name)
        {
            List<Category> categories = _db.Categories.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
            return !_db.Categories.Any(c => c.UserId == _userManager.GetUserId(User) &&
                                           c.SubCategories.Any(s => s.Name.ToLower().Trim() == name.ToLower().Trim()));
        }
        
        public async Task<bool>CheckOldPassword(string oldPassword)
        {
            User user = _userManager.GetUserAsync(User).Result;
            var result = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (result) return true;
            return false;
        }
        
        public bool ComparePasswords(string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword) return false;
            return true;
        }
    }
}