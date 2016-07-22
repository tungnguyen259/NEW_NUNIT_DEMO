using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nunit_Framework.PageActions.LoginPage
{
    public class LoginPageLowLevelActions : BaseStepsDefinition
    {
        public void OpenSite(string url)
        {
            if (url == "")
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AppUrl"] != null)
                {
                    url = System.Configuration.ConfigurationManager.AppSettings["AppUrl"];
                }
            }
            MaximizeWindow();
            GoToUrl(url);
        }

        public void EnterUserName(string username)
        {
            EnterValue("username textbox", username);
        }

        public void EnterPassWord(string password)
        {
            EnterValue("password textbox", password);
        }

        public void ClickOnLogin()
        {
            ClickOnElement("login button");
        }
        public void CheckWarningInvalidAcc(string error)
        {
            Assert.IsTrue(IsMessageDisplay(error));
        }

    }
}
