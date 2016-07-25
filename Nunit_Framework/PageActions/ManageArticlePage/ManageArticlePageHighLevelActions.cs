using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nunit_Framework.PageActions.ManageArticlePage
{
    public class ManageArticlePageHighLevelActions : ManageArticlePageLowLevelActions
    {
        public void VerifyCreatedArticleDisplayed(string articleName)
        {
            SearchArticle(articleName);
            Assert.IsTrue(IsArticleDisplayed(articleName));
        }

        public void DeleteArticle(string articleName)
        {
            if (articleName.Equals("All Articles"))
            {
                SelectNumberArticleOnTable("All");
                FindWebElement("select all articles checkbox").Click();
            }
            else
            {
                ClickOnDynamicElement("select articles checkbox", articleName);
            }

            ClickOnElement("trash button");
        }


    }
}