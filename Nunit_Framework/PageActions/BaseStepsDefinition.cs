using OpenQA.Selenium;
using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using OpenQA.Selenium.Interactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;

namespace Nunit_Framework.PageActions
{
    public class BaseStepsDefinition : BrowserManager
    {
        private static string GetClassCaller(int level = 4)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            // .Name is the name only, .FullName includes the namespace
            string className = m.DeclaringType.Name;
            string methodName = m.Name;
            if (className.Contains("LowLevelActions"))
                return className.Replace("LowLevelActions", "");
            else
                return className.Replace("HighLevelActions", "");
        }

        public void ClickOnElement(string element)
        {
            FindWebElement(element).Click();
        }
        public void ClickOnDynamicElement(string control, string value)
        {
            FindDynamicWebElement(control, value).Click();
        }

        public void EnterValue(string control, string value)
        {
            FindWebElement(control).Clear();
            FindWebElement(control).SendKeys(value);
        }

        public void CheckControlNotExist(string control)
        {
            Assert.IsFalse(FindWebElement(control).Displayed);
        }

        /*
        public void SelectDropDown(string page, string control, string value) {
            SelectElement selector = new SelectElement(FindWebElement(page, control));
            selector.SelectByText(value);
        }
        */

        public class control
        {
            public string controlName { get; set; }
            public string type { get; set; }
            public string value { get; set; }
        }

        public string[] GetControlValue(string nameControl)
        {
            string page = GetClassCaller();
            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            string content = File.ReadAllText(path + @"\Interfaces\" + page + ".json");
            var result = new JavaScriptSerializer().Deserialize<List<control>>(content);
            string[] control = new string[2];
            foreach (var item in result)
            {
                if (item.controlName.Equals(nameControl))
                {
                    control[0] = item.type;
                    control[1] = item.value;
                    return control;
                }
            }
            return null;
        }

        public IWebElement FindWebElement(string name)
        {
            string[] control = GetControlValue(name);
            switch (control[0].ToUpper())
            {
                case "ID":
                    return Browser.FindElement(By.Id(control[1]));
                case "NAME":
                    return Browser.FindElement(By.Name(control[1]));
                case "CLASSNAME":
                    return Browser.FindElement(By.ClassName(control[1]));
                default:
                    {
                        //IWait<IWebDriver> wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(30.00));
                        //wait.Until(driver1 => ((IJavaScriptExecutor)Browser).ExecuteScript("return document.readyState").Equals("complete"));
                        return Browser.FindElement(By.XPath(control[1]));
                    }
                    
            }
        }


        public bool IsMessageDisplay(string message)
        {
            if (Browser.FindElement(By.XPath("//div[@class='alert-message']")).Displayed)
            {
                string webmessage = Browser.FindElement(By.XPath("//div[@class='alert-message']")).Text;
                if (webmessage.Equals(message))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void HoverMouse(string control)
        {
            IWebElement element = FindWebElement(control);
            Actions action = new Actions(Browser);
            action.MoveToElement(element).Perform();
        }

        public bool CheckIfDynamicWebElementDisplayed(string articleName)
        {
            if (FindDynamicWebElement("articles name", articleName).Displayed)
                return true;
            else
                return false;
        }

        private IWebElement FindDynamicWebElement(string name, string value)
        {
            string[] control = GetControlValue(name);
            string dynamiccontrol = string.Format(control[1].ToString(), value);
            return Browser.FindElement(By.XPath(dynamiccontrol));
        }


        public void SelectDropdownItem(string control, string contentitem, String value)
        {
            FindWebElement(control).Click();
            FindDynamicWebElement(contentitem, value).Click();
        }

        public void SwitchFrame(string frame)
        {
            Browser.SwitchTo().Frame(frame);
        }

        public void SwitchDefaultFrame()
        {
            Browser.SwitchTo().DefaultContent();
        }
    }
}