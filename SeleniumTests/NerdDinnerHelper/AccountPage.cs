using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace NerdDinnerHelper.cs
{
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