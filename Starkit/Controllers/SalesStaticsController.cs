using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class SalesStaticsController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;

        public SalesStaticsController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> VisualizeSalesActionResult(string sortParam)
        {
            User user = await _userManager.GetUserAsync(User);
            Restaurant restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == user.RestaurantId);
            List<Order> orders = restaurant.Orders.Where(o => o.Status != Status.Отказ && o.Status != Status.Новая)
                .ToList();
            switch (sortParam)
            {
                case "week":
                    orders = orders.Where(o => o.OrderTime.Date >= DateTime.Now.Date.AddDays(-7)).ToList();
                    break;
                case "month":
                    orders = orders.Where(o => o.OrderTime.Date >= DateTime.Now.Date.AddMonths(-1)).ToList();
                    break;
                case "threeMonth":
                    orders = orders.Where(o => o.OrderTime.Date >= DateTime.Now.Date.AddMonths(-3)).ToList();
                    break;
                case "year":
                    orders = orders.Where(o => o.OrderTime.Date >= DateTime.Now.Date.AddYears(-1)).ToList();
                    break;
                default:
                    orders = orders.Where(o => o.OrderTime.Date == DateTime.Now.Date).ToList();
                    break;
            }
            return Json(orders.OrderBy(o => o.OrderTime));
        }

        [HttpGet]
        public async Task<IActionResult> GetPopularGoods()
        {
            User user = await _userManager.GetUserAsync(User);
            Restaurant restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == user.RestaurantId);
            List<Order> orders = restaurant.Orders.Where(o => o.Status != Status.Отказ && o.Status != Status.Новая)
                .ToList();
            IEnumerable<OrderProduct> dishes = orders.Where(o => o.OrdersProducts != null)
                .SelectMany(o => o.OrdersProducts.Where(op => op.Dish != null));
            var popularDish = dishes.GroupBy(d => d.Dish.Name)
                .Select(g => new
                {
                    Name = g.First().Dish.Name,
                    Qantity = g.Sum(s => s.Quantity),
                    Sum = g.Sum(s => s.Quantity) * g.First().Dish.Cost
                }).OrderByDescending(d => d.Qantity).First();
            IEnumerable<OrderProduct> menu = orders.Where(o => o.OrdersProducts != null)
                .SelectMany(o => o.OrdersProducts.Where(op => op.Menu != null));
            var popularMenu = menu.GroupBy(d => d.Menu.Name)
                .Select(g => new
                {
                    Name = g.First().Menu.Name,
                    Qantity = g.Sum(s => s.Quantity),
                    Sum = g.Sum(s => s.Quantity) * g.First().Menu.Cost
                }).OrderByDescending(m => m.Qantity).First();
            IEnumerable<OrderProduct> stocks = orders.Where(o => o.OrdersProducts != null)
                .SelectMany(o => o.OrdersProducts.Where(op => op.Stock != null));
            var popularStock = stocks.GroupBy(d => d.Stock.Name)
                .Select(g => new
                {
                    Name = g.First().Stock.Name,
                    Qantity = g.Sum(s => s.Quantity),
                    Sum = g.Sum(s => s.Quantity) * g.First().Stock.Cost
                }).OrderByDescending(s => s.Qantity).First();
            List<Object> popularProducts = new List<object>();
            popularProducts.Add(popularDish);
            popularProducts.Add(popularMenu);
            popularProducts.Add(popularStock);
            return Json(true);
        }
    }
}