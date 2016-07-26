using NUnit.Framework;
using Nunit_Framework.PageActions;
using System;

namespace Nunit_Framework.Testcases.Articles
{
    [TestFixture]
    public class ArticlesPage_002 : BaseTestMethods
    {
        public string ArticleTitle = TestData.ARTICLETITLEDEFAULT + DateTime.Now.ToString("MM/dd/yy H:mm:ss");
        public string ArticleTitleEdited = TestData.ARTICLETITLEDEFAULT + "_Edited" + DateTime.Now.ToString("MM/dd/yy H:mm:ss");

        [SetUp]
        public void setup()
        {
            StartTest();
        }
        [Test]
        public void TC004_EditAnArticle()
        {
            stepLogging("1. Navigate to the URL: http://capability.demojoomla.com:81/administrator/index.php");
            stepLogging("2. Enter valid username into Username field");
            stepLogging("3. Enter valid password into Password field");
            stepLogging("4. Click on 'Log in' button");
            Pages.LoginPage.LaunchSite();
            Pages.LoginPage.LoginToJoomlaSite(TestData.USERNAME, TestData.PASSWORD);

            stepLogging("5. Select Content > Article Manager");
            Pages.MainPage.SelectMainPageMenu("content menu/articles menu");

            stepLogging("6. Click on 'New' icon of the top right toolbar");
            Pages.ManageArticlePage.SelectButtonsOnArticlesPage("New");

            stepLogging("7. Enter a title on 'Title' field");
            stepLogging("8. Select an item from the 'Category' dropdown list");
            stepLogging("9. Enter value on 'Article Text' text area");
            stepLogging("10. Click on 'Save & Close' icon of the top right toolbar");
            Pages.CreateEditArticlePage.FillAndSubmitArticleValues(ArticleTitle, TestData.ARTICLECATEGORYDEFAULT, TestData.ARTICLETEXTDEFAULT, "Save & Close");

            stepLogging("11. Select the Article created above");
            Pages.ManageArticlePage.ClickArticle(ArticleTitle);

            stepLogging("12. Edit Article Information");
            Pages.CreateEditArticlePage.FillAndSubmitArticleValues(ArticleTitleEdited, TestData.ARTICLECATEGORYDEFAULT, TestData.ARTICLETEXTDEFAULT, "Save & Close");

            stepLogging("13. Verify the article is edited saved successfully");
            Pages.ManageArticlePage.IsArticleSuccessfullyCreatedUpdatedMessageDisplayed(TestData.SUCCESS_MESSAGE);
            Pages.ManageArticlePage.IsArticleDisplayed(ArticleTitleEdited);

            stepLogging("Post Condition: Delete Created Article");
            Pages.ManageArticlePage.DeleteArticle(ArticleTitleEdited);
        }
    }
}