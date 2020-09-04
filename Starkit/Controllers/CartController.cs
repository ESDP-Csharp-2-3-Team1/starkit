using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class CartController : Controller
    {
        private readonly StarkitContext _db;

        public CartController(StarkitContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Buy(string id, int quantity, string name)
        {
            Dish dish = new Dish();
            Menu menu = new Menu();
            Stock stock = new Stock();
            switch (name)
            {
                case "dish":
                    dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
                    break;
                case "menu":
                    menu = _db.Menu.FirstOrDefault(m => m.Id == id);
                    break;
                default:
                    stock = _db.Stocks.FirstOrDefault(s => s.Id == id);
                    break;
            }
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                switch (name)
                {
                    case "dish":
                        cart.Add(new Item { Dish = dish, Quantity = quantity });        
                        break;
                    case "menu":
                        cart.Add(new Item { Menu = menu, Quantity = quantity });   
                        break;
                    default:
                        cart.Add(new Item { Stock = stock, Quantity = quantity });   
                        break;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id, name);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;
                }
                else
                {
                    switch (name)
                    {
                        case "dish":
                            cart.Add(new Item { Dish = dish, Quantity = quantity });        
                            break;
                        case "menu":
                            cart.Add(new Item { Menu = menu, Quantity = quantity });   
                            break;
                        default:
                            cart.Add(new Item { Stock = stock, Quantity = quantity });   
                            break;
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return Ok();
        }
        
        public IActionResult Remove(string id, string name)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id, name);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(string id, string name)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            switch (name)
            {
                case "dish":
                    if (!cart.Any(c => c.Dish != null))
                        return -1;
                    break;
                case "menu":
                    if (!cart.Any(c => c.Menu != null))
                        return -1;
                    break;
                default:
                    if (!cart.Any(c => c.Stock != null))
                        return -1;
                    break;
            }
            for (int i = 0; i < cart.Count; i++)
            {
                switch (name)
                {
                    case "dish":
                        if (cart[i].Dish != null)
                            if (cart[i].Dish.Id.Equals(id))
                                return i;
                        break;
                    case "menu":
                        if (cart[i].Menu != null)
                            if (cart[i].Menu.Id.Equals(id))
                                return i;
                        break;
                    default:
                        if (cart[i].Stock != null)
                            if (cart[i].Stock.Id.Equals(id))
                                return i;
                        break;
                }
            }
            return -1;
        }

        public IActionResult ChangeQuantity(string id, int quantity, string name)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id, name);
            cart[index].Quantity = quantity;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("GetContentCard");
        }

        public IActionResult GetContentCard()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            decimal total = 0;
            IEnumerable<Item> items = new List<Item>();
            if (cart == null)
                cart = new List<Item>();
            ViewBag.cart = cart;
            if (cart.Any(item => item.Dish != null))
            {
                items = cart.Where(c => c.Dish != null);
                total += items.Sum(i => i.Dish.Cost * i.Quantity);
            }
            if (cart.Any(item => item.Menu != null))
            {
                items = cart.Where(c => c.Menu != null);
                total += items.Sum(i => i.Menu.Cost * i.Quantity);
            }
            if (cart.Any(item => item.Stock != null))
            {
                items = cart.Where(c => c.Stock != null);
                total += items.Sum(i => i.Stock.Cost * i.Quantity);
            }
            ViewBag.total = total;
            return PartialView("PartialView/CartContentPartialView");
        }

        public IActionResult GetTotal()
        {
            decimal total = 0;
            IEnumerable<Item> items = new List<Item>();
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
                cart = new List<Item>();
            if (cart.Any(item => item.Dish != null))
            {
                items = cart.Where(c => c.Dish != null);
                total += items.Sum(i => i.Dish.Cost * i.Quantity);
            }
            if (cart.Any(item => item.Menu != null))
            {
                items = cart.Where(c => c.Menu != null);
                total += items.Sum(i => i.Menu.Cost * i.Quantity);
            }
            if (cart.Any(item => item.Stock != null))
            {
                items = cart.Where(c => c.Stock != null);
                total += items.Sum(i => i.Stock.Cost * i.Quantity);
            }
            return Json(total);
        }
    }
}