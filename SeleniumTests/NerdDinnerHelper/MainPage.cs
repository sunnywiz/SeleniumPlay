using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NerdDinnerHelper.cs
{
    public class MainPage : PageBase
    {
        public MainPage(IWebDriver driver)
            : base(driver)
        {
            Wait.Until(d => d.Title == "Nerd Dinner");
        }

        public static MainPage NavigateDirectly(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://www.nerddinner.com");
            return new MainPage(driver);
        }

        public IWebElement LocationToSearch
        {
            get { return Driver.FindElement(By.Id("Location")); }
        }

        public IWebElement SearchButton
        {
            get { return Driver.FindElement(By.Id("search")); }
        }

        public List<IWebElement> PopularDinnerSearchResults
        {
            get { return Driver.FindElements(By.ClassName("dinnerItem")).ToList(); }
        }

        public IWebElement ViewAllDinnersLink
        {
            get
            {
                var searchTextDiv = (from e in Driver.FindElements(By.ClassName("search-text"))
                                     where e.TagName == "div"
                                     select e).FirstOrDefault();
                if (searchTextDiv == null) throw new NotFoundException("div.search-text not found");
                var viewAllDinnerslink = searchTextDiv.FindElement(By.TagName("a"));

                return viewAllDinnerslink;
            }
        }

        public IWebElement LogonLink
        {
            get
            {
                var logindisplaydiv = Driver.FindElement(By.Id("logindisplay"));
                if (logindisplaydiv == null) throw new NotFoundException("#logindisplay not found");
                return logindisplaydiv.FindElement(By.TagName("a"));
            }
        }
    }

    public class AccountPage : PageBase
    {
        public AccountPage(IWebDriver driver)
            : base(driver)
        {
            Wait.Until(d => d.Title == "Log On");
        }

        public List<OpenIdProvider> OpenIdProviders
        {
            get
            {
                var openIdProvidersUl = Driver.FindElement(By.ClassName("OpenIdProviders"));
                return (from oip in openIdProvidersUl.FindElements(By.ClassName("OPButton"))
                        select new OpenIdProvider(oip)).ToList();
            }
        }

        public class OpenIdProvider
        {
            private readonly IWebElement _rootElement;

            public OpenIdProvider(IWebElement rootElement)
            {
                _rootElement = rootElement;
            }

            public IWebElement RootElement { get { return _rootElement; } }

            public string FunkyUrl 
            {
                get { return RootElement.GetAttribute("id");  }
            }

            public bool IsLoggedIn
            {
                get
                {
                    string clars = RootElement.GetAttribute("class");
                    return clars.Contains("loginSuccess");
                }
            }
        }
    }
}
