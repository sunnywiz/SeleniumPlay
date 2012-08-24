using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
                driver.Navigate().GoToUrl("http://www.nerddinner.com");
                var input = driver.FindElement(By.Id("Location"));
                input.SendKeys("40056");
                var search = driver.FindElement(By.Id("search"));
                search.Click();
                var results = driver.FindElements(By.ClassName("dinnerItem"));
                // at this point, i don't know what to do, as there's never any search results.   
                Assert.AreEqual(0, results.Count,
                                "No dinners should be found.. omg, if this works, then its worth it to change the test");
            }
        }

        [Test]
        public void MainPage_CanSeeAllDinners_ShouldFindAFew()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("http://www.nerddinner.com");

                var searchTextDiv = (from e in driver.FindElements(By.ClassName("search-text"))
                                     where e.TagName == "div"
                                     select e).FirstOrDefault();
                Assert.IsNotNull(searchTextDiv, "Should find search text which has the view all link");
                var viewAllDinnerslink = searchTextDiv.FindElement(By.TagName("a"));
                Assert.IsNotNull(viewAllDinnerslink, "should find the view all upcoming dinners link");
                viewAllDinnerslink.Click();

                // now on /Dinners page
                var upcomingDinnersUl = (from e in driver.FindElements(By.ClassName("upcomingdinners"))
                                          where e.TagName == "ul"
                                          select e).FirstOrDefault();
                Assert.IsNotNull(upcomingDinnersUl,"should find a Upcoming Dinners UL");
                var dinnerLinks = upcomingDinnersUl.FindElements(By.TagName("a")); 
                Assert.Greater(dinnerLinks.Count, 0, "There should be at least one dinner out there.. come on guys");
            }
        }
    }
}
