using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class OrdersController : Controller
    {
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
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                foreach (var item in cart)
                {
                    
                }
                return Json(false);   
            }
            return PartialView("PartialViews/ToOrderModalWindowPartialView", order);
        }
    }
}