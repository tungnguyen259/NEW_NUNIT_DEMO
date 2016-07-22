using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nunit_Framework.PageActions
{
    public class CreateEditArticlePageHighLevelActions : CreateEditArticlePageLowLevelActions
    {
        /*
        public void CreateArticle(string title, string category, string articleText, string button) {
            FilterArticleValues(title, category, articleText);
            ClickButton(button);
        }

        public void EditArticle(string title, string category, string articleText, string button) {
            FilterArticleValues(title, category, articleText);
            ClickButton(button);
        }
        */

        public void FillArticleValues(string title, string category, string articleText)
        {
            if (!string.IsNullOrEmpty(title))
                InputTitle(title);
            if (!string.IsNullOrEmpty(category))
                SelectCategory(category);
            if (!string.IsNullOrEmpty(articleText))
                InputArticleText(articleText);
        }

    }
}
