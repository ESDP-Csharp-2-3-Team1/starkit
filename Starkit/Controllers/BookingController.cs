using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class BookingController : Controller
    {
        // GET
        private StarkitContext _db;
        private UserManager<User> _userManager;

        public BookingController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }

            List<Booking> bookings = _db.Bookings.Where(t => t.RestaurantId == user.RestaurantId).ToList();
            return View(bookings);
        }

        [Authorize(Roles = "SuperAdmin,Registrant")]

        [HttpGet]
        public async Task<IActionResult> Create()
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
        
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                booking.RestaurantId = user.RestaurantId;
                _db.Entry(booking).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(booking);
        }
        
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Booking booking = new Booking(){Id = id};
            _db.Entry(booking).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public IActionResult Edit(string id)
        {
            Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                EditBookingViewModel model = new EditBookingViewModel
                {
                    Id = booking.Id,
                    Date = booking.Date,
                    BookFrom = booking.BookFrom,
                    BookTo = booking.BookTo,
                    Comment = booking.Comment,
                    ClientName = booking.ClientName,
                    ClientSurname = booking.ClientSurname,
                    PhoneNumber = booking.PhoneNumber,
                    Email = booking.Email,
                    Pax = booking.Pax,
                    State = booking.State
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public async Task<IActionResult> Edit(EditBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                var booking = _db.Bookings.FirstOrDefault(b => b.Id == model.Id);
                if (booking != null)
                {
                    booking.Date = model.Date;
                    booking.BookFrom = model.BookFrom;
                    booking.BookTo = booking.BookTo;
                    booking.Comment = booking.Comment;
                    booking.ClientName = booking.ClientName;
                    booking.ClientSurname = booking.ClientSurname;
                    booking.PhoneNumber = booking.PhoneNumber;
                    booking.Email = booking.Email;
                    booking.Pax = booking.Pax;
                    booking.State = booking.State;

                    _db.Entry(booking).State = EntityState.Modified;
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
        
    }

}