using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private int pageSize = 5;

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
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
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
        
        public async Task<IActionResult> GetBookings(string name, int page = 1, SortState sortOrder = SortState.AddTimeAsc)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            var bookings = _db.Bookings.Where(b => b.RestaurantId == userId);
            
            if (!string.IsNullOrEmpty(name))
                bookings = bookings.Where(b => b.ClientName.ToLower().Contains(name.ToLower()));

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    bookings = bookings.OrderByDescending(b => b.ClientName);
                    break;
                case SortState.DateAsc:
                    bookings = bookings.OrderBy(b => b.Date);
                    break;
                case SortState.DateDesc:
                    bookings = bookings.OrderByDescending(b => b.Date);
                    break;
                case SortState.PaxAsc:
                    bookings = bookings.OrderBy(b => b.Pax);
                    break;
                case SortState.PaxDesc:
                    bookings = bookings.OrderByDescending(b => b.Pax);
                    break;
                case SortState.TimeAsc:
                    bookings = bookings.OrderBy(b => b.BookFrom);
                    break;
                case SortState.TimeDesc:
                    bookings = bookings.OrderByDescending(b => b.BookTo);
                    break;
                default:
                    bookings = bookings.OrderBy(b => b.ClientName);
                    break;
            }

            var count = await bookings.CountAsync();
            var items = await bookings.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            if (items.Count == 0 && page != 1)
            {
                page = page - 1;
                items = await bookings.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            var viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                Bookings = items
            };

            return PartialView("PartialViews/LIstBookingsPartialView", viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            var bookings = new List<Booking>();
            foreach (var id in ids)
            {
                bookings.Add(_db.Bookings.FirstOrDefault(b => b.Id == id));
            }

            if (bookings.Count == 1)
                _db.Bookings.Remove(bookings[0]);
            else
                _db.Bookings.RemoveRange(bookings);
            await _db.SaveChangesAsync();
            return Json(true);
        }
        

        [HttpGet]
        public IActionResult Details(string id)
        {
            Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            return PartialView("PartialViews/DetailsBookingModalWindowPartialView", booking);

        }
        
    }

}