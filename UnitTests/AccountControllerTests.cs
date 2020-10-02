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
            _basicSteps.DataForWrongAuthorization();
            Assert.True(_basicSteps.IsElementFound("Неверная попытка входа в систему"));
        }

        [Fact]
        public void LoginThreeTimesWrongModelDataReturnsCaptcha()
        {
            _basicSteps.DataForWrongAuthorization();
            _driver.FindElement(By.Id("img-captcha"));
        }
    }
}