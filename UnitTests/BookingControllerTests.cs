using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Starkit.Controllers;
using Starkit.Models;
using Starkit.Models.Data;
using Xunit;

namespace UnitTests
{
    partial interface IRepository
    {
        IEnumerable<BookingTable> GetAllBookingTable();
    }
    
    public class BookingController : Controller
    {
        IRepository repo;
        public BookingController(IRepository r)
        {
            repo = r;
        }
        public IActionResult Index()
        {
            return View("Index", repo.GetAllBookingTable());
        }
    }
    public class BookingControllerTests
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
                        
        [Fact]
        public void IndexViewResultNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo=>repo.GetAllBookingTable()).Returns(GetTestBookings());
            var controller = new BookingController(mock.Object);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public void IndexViewNameEqualIndex()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo=>repo.GetAllBookingTable()).Returns(GetTestBookings());
            var controller = new BookingController(mock.Object);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.Equal("Index", result?.ViewName);
        }
                
        [Fact]
        public void IndexReturnsAViewResultWithAListOfBookings()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo=>repo.GetAllBookingTable()).Returns(GetTestBookings());
            var controller = new BookingController(mock.Object);
         
            // Act
            var result = controller.Index();
         
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookingTable>>(viewResult.Model);
            Assert.Equal(GetTestBookings().Count, model.Count());
        }
        private List<BookingTable> GetTestBookings()
        {
            var rand = new Random();
            
            var bookings = new List<BookingTable>
            {
                new BookingTable
                {
                    Id = Guid.NewGuid().ToString(), 
                    BookingId = Guid.NewGuid().ToString(),
                    TableId = rand.Next(1, 100),
                },
                new BookingTable
                {
                    Id = Guid.NewGuid().ToString(), 
                    BookingId = Guid.NewGuid().ToString(),
                    TableId = rand.Next(1, 100),
                },
                new BookingTable
                {
                    Id = Guid.NewGuid().ToString(), 
                    BookingId = Guid.NewGuid().ToString(),
                    TableId = rand.Next(1, 100),
                },
                new BookingTable
                {
                    Id = Guid.NewGuid().ToString(), 
                    BookingId = Guid.NewGuid().ToString(),
                    TableId = rand.Next(1, 100),
                }
            };
            return bookings;
        }
    }
}