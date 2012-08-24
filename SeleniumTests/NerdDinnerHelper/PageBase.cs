using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NerdDinnerHelper.cs
{
    public abstract class PageBase
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }

        protected PageBase(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
        }
    }
}