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
    public interface IRepository
    {
        IEnumerable<Order> GetAll();
        Order Get(int id);
        void Create(Order order);
    }
    
    public class OrdersController : Controller
    {
        IRepository repo;
        public OrdersController(IRepository r)
        {
            repo = r;
        }
        public IActionResult Index()
        {
            return View("Index", repo.GetAll());
        }
    }
    
    public class OrdersControllerTests
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
                
        [Fact]
        public void IndexViewResultNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo=>repo.GetAll()).Returns(GetTestOrders());
            var controller = new OrdersController(mock.Object);
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
            mock.Setup(repo=>repo.GetAll()).Returns(GetTestOrders());
            var controller = new OrdersController(mock.Object);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.Equal("Index", result?.ViewName);
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAListOfOrders()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo=>repo.GetAll()).Returns(GetTestOrders());
            var controller = new OrdersController(mock.Object);
 
            // Act
            var result = controller.Index();
 
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Order>>(viewResult.Model);
            Assert.Equal(GetTestOrders().Count, model.Count());
        }
        private List<Order> GetTestOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = "Алмат", 
                    ContactNumber = "7 707 141 18 19", 
                    Address = "seyderaly98@gmail.com", 
                    OrderTime = DateTime.Today, 
                    Status = Status.Новая
                },
                new Order
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = "Самат", 
                    ContactNumber = "7 708 871 21 98", 
                    Address = "muratsamat090598@gmail.com", 
                    OrderTime = DateTime.Today, 
                    Status = Status.Новая
                },
                new Order
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = "Самал", 
                    ContactNumber = "7 700 766 76 80", 
                    Address = "samal.zhex@gmail.com", 
                    OrderTime = DateTime.Today, 
                    Status = Status.Новая
                },
                new Order
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = "Рашит", 
                    ContactNumber = "7 701 711 9967", 
                    Address = "rashit.nurzhanov@gmail.com", 
                    OrderTime = DateTime.Today, 
                    Status = Status.Новая
                },
            };
            return orders;
        }

    }
}