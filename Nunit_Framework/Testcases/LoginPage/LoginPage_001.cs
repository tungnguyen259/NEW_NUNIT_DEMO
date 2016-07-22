using NUnit.Framework;
using Nunit_Framework.PageActions;

namespace Nunit_Framework.Testcases.LoginPage
{
    [TestFixture]
    public class LoginPage_001 : BaseTestMethods
    {
        [SetUp]
        public void SetUp()
        {
            StartTest();
        }

        [Test]
        public void TC001_LoginToJoomlaWithInValidAccount()
        {
            stepLogging("Login to joomla site with invalid account");
            Pages.LoginPage.LoginToJoomlaSite(TestData.INVUSERNAME, TestData.INVPASSWORD);

            stepLogging("Check user cannot login to joomla site");
            Pages.LoginPage.CheckInvalidAccountMgsDisplay(TestData.ERRORINVALIDACC);
        }
        [Test]
        public void TC002_LoginToJoomlaWithValidAccount()
        {
            stepLogging("Login to joomla site with valid account");
            Pages.LoginPage.LoginToJoomlaSite(TestData.USERNAME, TestData.PASSWORD);

            stepLogging("Check user can login to joomla site successful");
            Pages.MainPage.CheckMainPageDisplay();
        }
    }
}
