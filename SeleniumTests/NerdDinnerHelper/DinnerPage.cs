using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace NerdDinnerHelper.cs
{
    public class DinnerPage : PageBase
    {
        public DinnerPage(IWebDriver driver) : base(driver)
        {
            Wait.Until(d => d.FindElement(By.ClassName("entry-title")).Text == "Upcoming Dinners");
        }

        public List<IWebElement> UpcomingDinners
        {
            get
            {
                var upcomingDinnersUl = (from e in Driver.FindElements(By.ClassName("upcomingdinners"))
                                         where e.TagName == "ul"
                                         select e).FirstOrDefault();
                if (upcomingDinnersUl == null) throw new NotFoundException("could not find ul.upcomingdinners");
                return upcomingDinnersUl.FindElements(By.TagName("a")).ToList(); 
            }
        }
    }
}