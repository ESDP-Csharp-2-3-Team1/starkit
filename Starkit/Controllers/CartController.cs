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
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Dish.Cost * item.Quantity);
            return View();
        }
        
        public IActionResult Buy(string id, int quantity)
        {
            Dish dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Dish = dish, Quantity = quantity });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;
                }
                else
                {
                    cart.Add(new Item { Dish = dish, Quantity = quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Dish.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}