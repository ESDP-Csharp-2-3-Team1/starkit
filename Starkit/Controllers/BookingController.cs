using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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
            var bookings = _db.Bookings.Where(t => t.RestaurantId == user.RestaurantId).ToList();
               
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
            ViewBag.Tables = _db.Tables.Where(t => t.RestaurantId == user.RestaurantId);
            return View();
        }
        
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking, int[] tableId)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                
                booking.RestaurantId = user.RestaurantId;
                booking.CreatorId = user.Id;
                _db.Entry(booking).State = EntityState.Added;
                foreach (var tId in tableId)
                {
                    BookingTable bookingTable = new BookingTable()
                    {
                        BookingId = booking.Id,
                        TableId = tId
                    };
                    _db.Entry(bookingTable).State = EntityState.Added;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public IActionResult Edit(string id)
        {
            User user = _userManager.FindByIdAsync(_userManager.GetUserId(User)).Result;
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                user =  _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur).Result;
            Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                EditBookingViewModel model = new EditBookingViewModel
                {
                    Id = booking.Id,
                    Date = booking.Date,
                    Comment = booking.Comment,
                    ClientName = booking.ClientName,
                    PhoneNumber = booking.PhoneNumber,
                    Email = booking.Email,
                    Pax = booking.Pax,
                    State = booking.State,
                    Tables = _db.Tables.Where(t => t.RestaurantId == user.RestaurantId).ToList(),
                    BookingTables = _db.BookingTables.Where(bt => bt.BookingId == booking.Id).ToList()
                };
                
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public async Task<IActionResult> Edit(EditBookingViewModel model, int tableId)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                var booking = _db.Bookings.FirstOrDefault(b => b.Id == model.Id);

                if (booking != null)
                {
                    if (booking.BookingTables.All(b => b.TableId != tableId))
                    {
                        BookingTable bookingTable = new BookingTable()
                        {
                            BookingId = booking.Id,
                            TableId = tableId
                        };
                        _db.Entry(bookingTable).State = EntityState.Added;
                    }
                    booking.Date = model.Date;
                    booking.Comment = model.Comment;
                    booking.ClientName = model.ClientName;
                    booking.PhoneNumber = model.PhoneNumber;
                    booking.Email = model.Email;
                    booking.Pax = model.Pax;
                    booking.State = model.State;
                    booking.EditorId = user.Id;
                    booking.EditedDate = DateTime.Now;
                    _db.Entry(booking).State = EntityState.Modified;
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
        
        public async Task<IActionResult> GetBookings(int name, string state, int page = 1, SortState sortOrder = SortState.AddTimeAsc)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            var bookings = _db.Bookings.Where(b => b.RestaurantId == user.RestaurantId);
            
            var query = _db.BookingTables.Join(_db.Tables, bt => bt.TableId, t => t.Id,
                (bt, t) => new {Bt = bt,  Table = t}).Select(bt_t => new {bt_t.Table.Id, bt_t.Bt.BookingId}).ToList();
            if (state != null)
            {
                var s = (BookingStatus) Enum.Parse(typeof(BookingStatus), state, true);
                bookings = bookings.Where(d => d.State == s);
            }
            
            if (name != 0)
                foreach (var q in query)
                {
                    if (q.Id == name)
                    {
                        bookings = bookings.Where(b => b.Id == q.BookingId);
                    }
                    
                }

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
                BookingTablesFilterViewModel = new BookingTablesFilterViewModel(state, name),
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                Bookings = items
            };

            return PartialView("PartialViews/LIstBookingsPartialView", viewModel);
        }
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpPost]
        public async Task<IActionResult> ChangeBookingsState(string[] ids, BookingStatus state)
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
                Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
                booking.State = state;
                bookings.Add(booking);
            }

            if (bookings.Count == 1)
                _db.Bookings.Update(bookings[0]);
            else
                _db.Bookings.UpdateRange(bookings);
            await _db.SaveChangesAsync();
            return Json(true);
        }
        

        [HttpGet]
        public IActionResult Details(string id)
        {
            Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            return PartialView("PartialViews/DetailsBookingModalWindowPartialView", booking);

        }

        [Authorize(Roles = "SuperAdmin,Registrant")]
        public async Task<IActionResult> ChangeState(string id, BookingStatus state)
        {
            Booking booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.State = state;
                booking.BookingTables = _db.BookingTables.Where(bt => bt.BookingId == booking.Id).ToList();
                if (state == BookingStatus.Cancelled || state == BookingStatus.Done || state == BookingStatus.NoShow)
                {
                    foreach (var b in booking.BookingTables)
                    {
                        b.IsDeleted = true;
                        Table table = _db.Tables.FirstOrDefault(t => t.Id == b.TableId);
                        table.State = TableState.Available;
                    }
                    _db.Entry(booking.BookingTables).State = EntityState.Modified;
                }
                _db.Entry(booking).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(booking.State);
            }

            return Json(false);
        }
        
        public IActionResult Book(int id, string date, string customDate, string timeFrom, string timeTo)
        {
            CreateBookingViewModel bookingTable = new CreateBookingViewModel()
            {
                TableId = id,
                BookFrom = timeFrom,
                BookTo = timeTo,
                Date = date,
                CustomDate = customDate
            };
            return PartialView("PartialViews/BookTableModalPartialView", bookingTable);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(CreateBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Table table = _db.Tables.FirstOrDefault(t => t.Id == model.TableId);
                Booking booking = new Booking()
                {
                    ClientName = model.ClientName,
                    BookFrom = model.BookFrom,
                    BookTo = model.BookTo,
                    Comment = model.Comment,
                    RestaurantId = table.RestaurantId,
                    Pax = model.Pax,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                };
                if (model.Date == "today")
                {
                    booking.Date = DateTime.Today.ToShortDateString();
                }
                else if (model.Date == "tomorrow")
                {
                    booking.Date = DateTime.Today.AddDays(1).ToShortDateString();
                }
                else if (model.Date == "custom")
                {
                    booking.Date = model.CustomDate;
                }
                else
                {
                    
                    return PartialView("PartialViews/BookTableModalPartialView", model);
                }

                if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "booking") == null)
                {
                    List<Booking> items = new List<Booking>();
                    items.Add(booking);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "booking", items);
                }
                else
                {
                    List<Booking> items = SessionHelper.GetObjectFromJson<List<Booking>>(HttpContext.Session, "booking");
                    items.Add(booking);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "booking", items);
                }
                _db.Entry(booking).State = EntityState.Added;
                BookingTable bookingTable = new BookingTable()
                {
                    BookingId = booking.Id,
                    TableId = model.TableId 
                };
                _db.Entry(bookingTable).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return Json(new{status = "success"});
            }
            
            return PartialView("PartialViews/BookTableModalPartialView", model);

        }
        
        public List<int> CheckTableAvailability(string date, string customDate, string timeFrom, string timeTo)
        {
            string bookingDate = DateTime.Today.ToShortDateString();
            if (date == "today")
            {
                bookingDate = DateTime.Today.ToShortDateString();
            }
            else if (date == "tomorrow")
            {
                bookingDate = DateTime.Today.AddDays(1).ToShortDateString();
            }
            else if (date == "custom")
            {
                bookingDate = customDate;
            }
            var bookFrom = Convert.ToInt32(timeFrom.Split(":")[0]);
            var bookTo = Convert.ToInt32(timeTo.Split(":")[0]);
            var tables = new List<int>();
            List<Booking> bookings = new List<Booking>();
            var query = _db.BookingTables.Join(_db.Bookings, bt => bt.BookingId, b => b.Id,
                (bt, b) => new {Bt = bt,  Booking = b}).ToList();
            foreach (var q in query)
            {
                var from = Convert.ToInt32(q.Booking.BookFrom.Split(":")[0]);
                var to = Convert.ToInt32(q.Booking.BookTo.Split(":")[0]);
                if (q.Booking.Date == bookingDate && from + 1 >= bookFrom && to + 1 >= bookTo)
                {
                    tables.AddRange(_db.Tables.Where(t => t.Id == q.Bt.TableId).Select(t => t.Id).ToList());
                }
            }

            return tables;
        }
        
        
        public IActionResult Cancel(string id)
        {
            List<Booking> bookings = SessionHelper.GetObjectFromJson<List<Booking>>(HttpContext.Session, "booking");
            Booking booking = bookings.FirstOrDefault(b => b.Id == id);
            booking.State = BookingStatus.Cancelled;
            bookings.Remove(booking);
            _db.Bookings.Update(booking);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "booking", bookings);
            return RedirectToAction("Index", "Site");
        }

        public IActionResult Booking()
        {
            var booking = SessionHelper.GetObjectFromJson<List<Booking>>(HttpContext.Session, "booking");
            if (booking == null)
            {
                booking = new List<Booking>();

            }
            ViewBag.Booking = booking;

            return View();
        }
        
        public IActionResult GetTotal()
        {
            var booked = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "booking");
            if (booked == null)
                booked = new List<Item>();
            decimal total = booked.Count;
            return Json(total);
        }
    }

}