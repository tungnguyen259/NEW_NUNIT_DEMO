using Nunit_Framework.PageActions.LoginPage;
using Nunit_Framework.PageActions.MainPage;
using Nunit_Framework.PageActions.ManageArticlePage;
using OpenQA.Selenium.Support.PageObjects;

namespace Nunit_Framework.PageActions
{
    public class Pages
    {
        private static T GetPage<T>() where T : new()
        {
            var page = new T();
            PageFactory.InitElements(BrowserManager.Browser, page);
            return page;
        }

        public static LoginPageHighLevelActions LoginPage
        {
            get { return GetPage<LoginPageHighLevelActions>(); }
        }
        public static MainPageHighLevelActions MainPage
        {
            get { return GetPage<MainPageHighLevelActions>(); }
        }
        public static ManageArticlePageHighLevelActions ManageArticlePage
        {
            get { return GetPage<ManageArticlePageHighLevelActions>(); }
        }
        public static CreateEditArticlePageHighLevelActions CreateEditArticlePage
        {
            get { return GetPage<CreateEditArticlePageHighLevelActions>(); }
        }
    }
}
