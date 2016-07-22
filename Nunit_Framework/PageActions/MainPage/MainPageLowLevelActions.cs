using OpenQA.Selenium;
using System.Threading;

namespace Nunit_Framework.PageActions.MainPage
{
    public class MainPageLowLevelActions: BaseStepsDefinition
    {
        public void SelectMainPageMenu(string path)
        {
            int pos = path.IndexOf("/");
            string[] count = path.Split('/');
            int total = count.Length;
            Thread.Sleep(2000);
            ClickOnElement(count[0]);

            if (total == 3)
            {
                HoverMouse(count[1]);
                ClickOnElement(count[2]);

            }
            else
                ClickOnElement(count[1]);
    
        }
        public bool CheckHomeIconReturns()
        {
           return FindWebElement("home page icon").Displayed;

        }
    }
}