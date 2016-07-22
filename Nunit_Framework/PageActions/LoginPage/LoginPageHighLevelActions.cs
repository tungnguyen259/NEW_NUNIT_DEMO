using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nunit_Framework.PageActions.LoginPage
{
    public class LoginPageHighLevelActions : LoginPageLowLevelActions
    {
        public void LaunchSite()
        {
            OpenSite(TestData.URL);
        }

        public void LoginToJoomlaSite(string username, string password)
        {
            EnterUserName(username);
            EnterPassWord(password);
            ClickOnLogin();
        }

        public void CheckInvalidAccountMgsDisplay(string message)
        {
            CheckWarningInvalidAcc(message);
        }


    }
}
