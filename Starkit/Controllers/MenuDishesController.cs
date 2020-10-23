using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class MenuDishesController : Controller
    {
        private readonly StarkitContext _db;

        public MenuDishesController(StarkitContext db)
        {
            _db = db;
        }
        
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpPost]
        public async Task<IActionResult> Create(string dishId, List<string> menuIds)
        {
            IEnumerable<MenuDish> menuDishes = _db.MenuDishes.Where(m => m.DishId == dishId);
            if (menuDishes.Count() == 0 && menuIds.Count == 0)
                return Json(false);
            if (menuIds.Count != 0)
            {
                List<string> menuDishIds = new List<string>();
                foreach (var menuDish in menuDishes)
                    menuDishIds.Add(menuDish.MenuId);
                if (menuDishIds.SequenceEqual(menuIds))
                    return Json(false);
                if (menuDishes.Count() != 0)
                    _db.MenuDishes.RemoveRange(_db.MenuDishes.Where(m => m.DishId == dishId));
                foreach (var menuId in menuIds)
                { 
                    MenuDish menuDish = new MenuDish{MenuId = menuId, DishId = dishId}; 
                    _db.Entry(menuDish).State = EntityState.Added;
                }
            }
            else
                _db.MenuDishes.RemoveRange(_db.MenuDishes.Where(m => m.DishId == dishId));
            await _db.SaveChangesAsync();
            return Json(true);
        }
    }
}