using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace NerdDinnerHelper.cs
{
    class Program
    {
        static void Main()
        {
            var destinations = new Dictionary<string, string>();
            destinations["all"] = "alldinners";
            destinations["first"] = "alldinners firstdinner";
            destinations["login"] = "login";
            destinations["host"] = "login hostdinner";
            destinations["quit"] = "";
            IWebDriver driver = null;
            try
            {
                while (true)
                {
                    Console.WriteLine("where do you want to go today?");
                    foreach (var result in destinations.OrderBy(x => x.Key))
                        Console.WriteLine("{0,10}=>{1}", result.Key, result.Value);
                    string dest = Console.ReadLine() ?? string.Empty;
                    if (dest==String.Empty) break;

                    // old delimiter trick
                    if (destinations.ContainsKey(dest)) dest = " "+destinations[dest]+" ";

                    if (driver != null)
                    {
                        driver.Dispose();
                        driver = null; 
                    }

                    driver = new FirefoxDriver();
                    driver.Manage().Window.Position = new Point(30, 30);
                    driver.Manage().Window.Size = new Size(1024, 768);

                    GoSomewhere(driver, dest); 
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex);
                Console.WriteLine("Press RETURN to close the browser");
                Console.ReadLine();
            } finally
            {
                if (driver != null)
                {
                    driver.Dispose();
// ReSharper disable RedundantAssignment because we use it again in the loop
                    driver = null;  
// ReSharper restore RedundantAssignment
                } 
            }
         }

        private static void GoSomewhere(IWebDriver driver, string destination)
        {
            Console.WriteLine("Navigating to: "+destination);

            var mainPage = MainPage.NavigateDirectly(driver);
            if (destination.Contains(" alldinners "))
            {
                mainPage.ViewAllDinnersLink.Click();
                if (destination.Contains(" firstdinner "))
                {
                    var dinners = mainPage.PopularDinnerSearchResults; 
                    if (dinners.Count>0)
                    {
                        dinners[0].Click(); 
                    }
                }
            } else if (destination.Contains(" login "))
            {
                mainPage.LogonLink.Click();
                var accountPage = new AccountPage(driver);

                var googleLink = (from oip in accountPage.OpenIdProviders
                                  where oip.FunkyUrl == "https://www.google.com/accounts/o8/id"
                                  select oip).FirstOrDefault();
                if (googleLink != null)
                {
                        googleLink.RootElement.Click();
                    mainPage.Wait.Until(d => mainPage.LoginDisplayDiv.Text.Contains("Welcome"));

                    if (destination.Contains(" hostdinner "))
                    {
                        mainPage.HostDinner.Click(); 

                        // other stuff goes here eventually.
                    }
                }
            }
            Console.WriteLine("Done navigating");
        }
    }
}
