using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nunit_Framework.PageActions.ManageArticlePage
{
    public class ManageArticlePageLowLevelActions : BaseStepsDefinition
    {

        public void SearchArticle(string articleName)
        {
            EnterValue("search textbox", articleName);
            ClickOnElement("search button");
        }
        public void SortArticle(string sortValue)
        {
            SelectDropdownItem("sort arrow", "sort value", sortValue);

        }
        public void SelectNumberArticleOnTable(string numberArticle)
        {
            SelectDropdownItem("number article arrow", "article number", numberArticle);
        }
        public bool IsArticleDisplayed(string articleName)
        {
            return CheckIfDynamicWebElementDisplayed(articleName);
        }
        public void ClickArticleDynamicCheckbox(String control, String value)
        {
            ClickOnDynamicElement(control, value);
        }
        public void ClickArticle(string articleName)
        {
            ClickOnDynamicElement("articles name", articleName);
        }
        public void SelectButtonsOnArticlesPage(String button)
        {
            switch (button)
            {
                case "New":
                    ClickOnElement("new button");
                    break;
                case "Edit":
                    ClickOnElement("edit button");
                    break;
                case "Trash":
                    ClickOnElement("trash button");
                    break;
                case "Search":
                    ClickOnElement("search button");
                    break;
            }
        }
        public bool IsArticleSuccessfullyCreatedUpdatedMessageDisplayed()
        {
            string message = "Article successfully saved";
            return IsMessageDisplay(message);
        }
    }
}
