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

namespace Starkit.Controllers
{
    public class OrdersController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;

        public OrdersController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            User user = await _userManager.GetUserAsync(User);
            Restaurant restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == user.RestaurantId);
            if (restaurant != null)
                return PartialView("PartialViews/ListOrderPartialView", restaurant.Orders.OrderBy(o => o.OrderTime).ToList());
            return NotFound();
        }

        public IActionResult Add()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
                cart = new List<Item>();
            if (cart.Count != 0)
                return PartialView("PartialViews/ToOrderModalWindowPartialView", new Order());
            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            if (ModelState.IsValid)
            {
                if (_db.Orders.Any())
                    order.OrderNum = _db.Orders.ToList().Last().OrderNum + 1;
                else
                    order.OrderNum = 1;
                order.OrderTime = DateTime.Now;
                order.Status = Status.Новая;
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                foreach (var item in cart)
                {
                    if (item.Dish != null)
                    {
                        OrdersDishes ordersDishes = new OrdersDishes
                        {
                            OrderId = order.Id,
                            DishId = item.Dish.Id,
                            Quantity = item.Quantity,
                        };
                        _db.Entry(ordersDishes).State = EntityState.Added;
                        if (order.RestaurantId == null)
                            order.RestaurantId = item.Dish.RestaurantId;
                    }
                    else if (item.Menu != null)
                    {
                        OrdersMenu ordersMenu = new OrdersMenu
                        {
                            OrderId = order.Id,
                            MenuId = item.Menu.Id,
                            Quantity = item.Quantity
                        };
                        _db.Entry(ordersMenu).State = EntityState.Added;
                        if (order.RestaurantId == null)
                            order.RestaurantId = item.Menu.RestaurantId;
                    }
                    else if (item.Stock != null)
                    {
                        OrdersStocks ordersStocks = new OrdersStocks
                        {
                            OrderId = order.Id,
                            StockId = item.Stock.Id,
                            Quantity = item.Quantity
                        };
                        _db.Entry(ordersStocks).State = EntityState.Added;
                        if (order.RestaurantId == null)
                            order.RestaurantId = item.Stock.RestaurantId;
                    }
                }
                _db.Entry(order).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return Json(order.OrderNum);   
            }
            return PartialView("PartialViews/ToOrderModalWindowPartialView", order);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Order order = new Order{Id = id};
            _db.Entry(order).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return RedirectToAction("GetOrders");
        }
    }
}