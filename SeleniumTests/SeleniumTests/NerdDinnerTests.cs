using System;
using System.Linq;
using NUnit.Framework;
using NerdDinnerHelper.cs;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class NerdDinnerTests
    {
        private IWebDriver Driver;
        private MainPage MainPage; 

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            AbandonDriver();
        }

        [SetUp]
        public void TestSetup()
        {
            if (Driver == null) Driver = new FirefoxDriver();
            MainPage = MainPage.NavigateDirectly(Driver);
        }

        [TearDown]
        public void TestTeardown()
        {
            
        }

        private void AbandonDriver()
        {
            if (Driver != null) Driver.Dispose();
            Driver = null;
        }

        [Test]
        public void MainPage_CanSearchForDinners_ButNeverFindsAnyInKY()
        {
            MainPage.LocationToSearch.SendKeys("40056");
            MainPage.SearchButton.Click();
            var results = MainPage.PopularDinnerSearchResults;

            Assert.AreEqual(0, results.Count,
                            "No dinners should be found.. omg, if this works, then its worth it to change the test");

        }

        [Test]
        public void MainPage_CanSeeAllDinners_ShouldFindAFew()
        {
                MainPage.ViewAllDinnersLink.Click();

                var dinnerPage = new DinnerPage(Driver);
                Assert.Greater(dinnerPage.UpcomingDinners.Count, 0, "There should be at least one dinner out there.. come on guys");
        }

        [Test]
        public void CanLogInWithGoogle()
        {
            try
            {
                MainPage.LogonLink.Click();

                var accountPage = new AccountPage(Driver);
                var googleLink = (from oip in accountPage.OpenIdProviders
                                  where oip.FunkyUrl == "https://www.google.com/accounts/o8/id"
                                  select oip).FirstOrDefault();
                Assert.IsNotNull(googleLink,"should support google");
                Assert.IsFalse(googleLink.IsLoggedIn, "Because this is a test, should never be logged in");
                googleLink.RootElement.Click();

                // this one might need human intervention
                var wait = new WebDriverWait(Driver, TimeSpan.FromMinutes(5));
                wait.Until(d => MainPage.LoginDisplayDiv.Text.Contains("Welcome"));
            } finally
            {
                // we logged in, can't trust it now. 
                AbandonDriver(); 
            }
        }
    }
}
