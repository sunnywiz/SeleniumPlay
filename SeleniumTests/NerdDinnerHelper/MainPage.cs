using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

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
                return LoginDisplayDiv.FindElement(By.TagName("a"));
            }
        }

        public IWebElement LoginDisplayDiv
        {
            get { return Driver.FindElement(By.Id("logindisplay")); }
        }

        public IWebElement HostDinner
        {
            get { return Driver.FindElement(By.LinkText("Host Dinner")); }
        }
    }
}
