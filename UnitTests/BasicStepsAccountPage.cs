using System;
using OpenQA.Selenium;

namespace UnitTests
{
    public class BasicStepsAccountPage: IDisposable
    {
        private readonly IWebDriver _driver;
        private const string MainPageUrl = "http://localhost:5000";

        public BasicStepsAccountPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void GoToMainPage()
        {
            GoToUrl(MainPageUrl);
        }
        
        public void FillTextField(string fieldId, string inputText)
        {
            _driver.FindElement(By.Id(fieldId))
                .SendKeys(inputText);
        }
        
        public void ClickById(string id)
        {
            _driver.FindElement(By.Id(id)).Click();
        }
        
        public bool IsElementFound(string text)
        {
            var element = _driver.FindElement(By.XPath($"//*[contains(text(), '{text}')]"));
            return element != null;
        }
        
        public void DataForWrongAuthorization()
        {
            GoToMainPage();
            FillTextField("Email", "wrong@mail.ru");
            FillTextField("inputPassword", "wrongPassword"); 
            ClickById("login-btn");
        }
    }
}