using NUnit.Framework;
using Nunit_Framework.PageActions;
using Nunit_Framework.PageActions.MainPage;
using System;

namespace Nunit_Framework.Testcases.Articles
{
    [TestFixture]
    public class ArticlesPage_001 : BaseTestMethods
    {
        public string ArticleTitle = TestData.ARTICLETITLEDEFAULT + DateTime.Now.ToString("MM/dd/yy H:mm:ss");

        [SetUp]
        public void setup()
        {
            StartTest();
        }
        [Test]
        public void TC003_CreateAnArticle()
        {
            stepLogging("1. Navigate to the URL: http://capability.demojoomla.com:81/administrator/index.php");
            stepLogging("2. Enter valid username into Username field");
            stepLogging("3. Enter valid password into Password field");
            stepLogging("4. Click on 'Log in' button");
            PageActions.Pages.LoginPage.LoginToJoomlaSite(TestData.USERNAME, TestData.PASSWORD);

            stepLogging("5. Select Content > Article Manager");
            PageActions.Pages.MainPage.SelectMainPageMenu("content menu/articles menu");

            stepLogging("6. Click on 'New' icon of the top right toolbar");
            PageActions.Pages.ManageArticlePage.SelectButtonsOnArticlesPage("New");

            stepLogging("7. Enter a title on 'Title' field");
            stepLogging("8. Select an item from the 'Category' dropdown list");
            stepLogging("9. Enter value on 'Article Text' text area");
            PageActions.Pages.CreateEditArticlePage.FillArticleValues(ArticleTitle, TestData.ARTICLECATEGORYDEFAULT, TestData.ARTICLETEXTDEFAULT);

            stepLogging("10. Click on 'Save & Close' icon of the top right toolbar");
            PageActions.Pages.CreateEditArticlePage.ClickCreateEditArticlePageButton("Save & Close");

            stepLogging("11. Verify the article is saved successfully");
            PageActions.Pages.ManageArticlePage.IsArticleSuccessfullyCreatedMessageDisplayed();
            PageActions.Pages.ManageArticlePage.IsArticleDisplayed(ArticleTitle);

            stepLogging("Post Condition: Delete Created Article");
            PageActions.Pages.ManageArticlePage.DeleteArticle(ArticleTitle);
        }
    }
}