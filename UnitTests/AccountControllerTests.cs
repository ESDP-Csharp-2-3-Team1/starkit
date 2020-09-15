using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace UnitTests
{
    public class AccountControllerTests: IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly BasicStepsAccountPage _basicSteps;

        public AccountControllerTests()
        {
            _driver = new ChromeDriver();
            _basicSteps = new BasicStepsAccountPage(_driver);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void LoginWrongModelDataReturnsErrorMessageTest()
        {
            DataForWrongAuthorization();
            Assert.True(_basicSteps.IsElementFound("Неверная попытка входа в систему"));
        }

        public void DataForWrongAuthorization()
        {
            _basicSteps.GoToMainPage();
            _basicSteps.FillTextField("Email", "wrong@mail.ru");
            _basicSteps.FillTextField("inputPassword", "wrongPassword");
            _basicSteps.ClickById("login-btn");
        }
    }
}