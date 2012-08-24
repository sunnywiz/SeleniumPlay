using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NerdDinnerHelper.cs;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTests
{
    [TestFixture]
    public class NerdDinnerTests
    {
        [Test]
        public void MainPage_CanSearchForDinners_ButNeverFindsAnyInKY()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var mainPage = MainPage.NavigateDirectly(driver);
                mainPage.LocationToSearch.SendKeys("40056");
                mainPage.SearchButton.Click();
                var results = mainPage.PopularDinnerSearchResults; 

                Assert.AreEqual(0, results.Count,
                                "No dinners should be found.. omg, if this works, then its worth it to change the test");
            }
        }

        [Test]
        public void MainPage_CanSeeAllDinners_ShouldFindAFew()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var mainpage = MainPage.NavigateDirectly(driver);
                mainpage.ViewAllDinnersLink.Click();

                var dinnerPage = new DinnerPage(driver);
                Assert.Greater(dinnerPage.UpcomingDinners.Count, 0, "There should be at least one dinner out there.. come on guys");
            }
        }

        [Test]
        public void CanLogInWithGoogle()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var mainpage = MainPage.NavigateDirectly(driver);
                mainpage.LogonLink.Click();

                var accountPage = new AccountPage(driver);
                var googleLink = (from oip in accountPage.OpenIdProviders
                                  where oip.FunkyUrl == "https://www.google.com/accounts/o8/id"
                                  select oip).FirstOrDefault();
                Assert.IsFalse(googleLink.IsLoggedIn, "Because this is a test, should never be logged in"); 
                googleLink.RootElement.Click();
            }
        }
    }
}
