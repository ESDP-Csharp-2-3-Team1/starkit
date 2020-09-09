using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class OrdersController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
        private string _path = "items.json";
        private string _orderIdPath = "orderId.json";

        public OrdersController(StarkitContext db, UserManager<User> userManager)
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

        public IActionResult Details(string id)
        {
            List<Item> items = new List<Item>();
            Order order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order.OrdersDishes.Any())
                foreach (var orderDish in order.OrdersDishes)
                    items.Add(new Item{Dish = orderDish.Dish, Quantity = orderDish.Quantity});
            if (order.OrdersMenu.Any())
                foreach (var orderMenu in order.OrdersMenu)
                    items.Add(new Item{Menu = orderMenu.Menu, Quantity = orderMenu.Quantity});
            if (order.OrdersStocks.Any())
                foreach (var orderStock in order.OrdersStocks)
                    items.Add(new Item{Stock = orderStock.Stock, Quantity = orderStock.Quantity});
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            var orderIdJson = JsonConvert.SerializeObject(order.Id);
            System.IO.File.WriteAllText(_path, json);
            System.IO.File.WriteAllText(_orderIdPath, orderIdJson);
            return View(order);
        }

        public IActionResult ChangeQuantity(string id, int quantity)
        {
            string json = System.IO.File.ReadAllText(_path);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
            items.FirstOrDefault(i => i.Id == id).Quantity = quantity;
            json = JsonConvert.SerializeObject(items, Formatting.Indented);
            System.IO.File.WriteAllText(_path, json);
            return RedirectToAction("GetContentOrder");
        }

        public async Task<IActionResult> GetContentOrder()
        {
            string json = System.IO.File.ReadAllText(_path);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
            IEnumerable<Item> buf = new List<Item>();
            decimal total = 0;
            if (items.Any(item => item.Dish != null))
            {
                buf = items.Where(c => c.Dish != null);
                total += buf.Sum(i => i.Dish.Cost * i.Quantity);
            }
            if (items.Any(item => item.Menu != null))
            {
                buf = items.Where(c => c.Menu != null);
                total += buf.Sum(i => i.Menu.Cost * i.Quantity);
            }
            if (items.Any(item => item.Stock != null))
            {
                buf = items.Where(c => c.Stock != null);
                total += buf.Sum(i => i.Stock.Cost * i.Quantity);
            }
            ViewBag.total = total;
            return PartialView("PartialViews/ContentCartPartialView", items);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItem(string id)
        {
            string json = System.IO.File.ReadAllText(_path);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
            items.Remove(items.FirstOrDefault(i => i.Id == id));
            json = JsonConvert.SerializeObject(items, Formatting.Indented);
            System.IO.File.WriteAllText(_path, json);
            return RedirectToAction("GetContentOrder");
        }

        [HttpPost]
        public async Task<IActionResult> SaveChange()
        {
            string json = System.IO.File.ReadAllText(_path);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
            string orderIdJson = System.IO.File.ReadAllText(_orderIdPath);
            string orderId = JsonConvert.DeserializeObject<string>(orderIdJson);
            Order order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
            order.OrdersDishes.Clear();
            order.OrdersMenu.Clear();
            order.OrdersStocks.Clear();
            _db.OrdersDishes.RemoveRange(_db.OrdersDishes.Where(od => od.OrderId == null));
            _db.OrdersMenu.RemoveRange(_db.OrdersMenu.Where(om => om.OrderId == null));
            _db.OrdersStocks.RemoveRange(_db.OrdersStocks.Where(os => os.OrderId == null));
            if (items.Count != 0)
            {
                foreach (var item in items)
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
                    }
                }
                _db.Entry(order).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            else
            {
                _db.Entry(order).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return Json(false);
            }
            return Json(true);
        }
    }
}