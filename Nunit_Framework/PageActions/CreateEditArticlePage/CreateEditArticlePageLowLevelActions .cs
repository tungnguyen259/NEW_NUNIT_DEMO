using System.Reflection;

namespace Nunit_Framework.PageActions
{
    public class CreateEditArticlePageLowLevelActions : BaseStepsDefinition
    {

        public void ClickCreateEditArticlePageButton(string button)
        {
            switch (button.ToUpper())
            {
                case "SAVE":
                    {
                        ClickOnElement("save button");
                        break;
                    }
                case "SAVENEW":
                    {
                        ClickOnElement("save and new button");
                        break;
                    }
                case "CANCEL":
                    {
                        ClickOnElement("cancel button");
                        break;
                    }
                default:
                    {
                        ClickOnElement("save and close button");
                        break;
                    }
            }
        }
        public void EnterArticleAlias(string aliasName)
        {
            EnterValue("title textbox", aliasName);
        }
        public void EnterArticleContent(string content)
        {
            //SwitchToFrame("jform_articletext_ifr");
            ClickOnElement("content textbox");
            EnterValue("content textbox", content);
            //SwitchToDefaultContent();
        }

        public void InputTitle(string title)
        {
            EnterValue("title textbox", title);
        }

        public void SelectCategory(string categoryValue)
        {
            //SelectDropdownItem(page, "category dropdown", "category dropdown content", "Uncategorised");
            SelectDropdownItem("category dropdown", "category dropdown content", categoryValue);
        }

        public void InputArticleText(string text)
        {
            SwitchFrame("jform_articletext_ifr");
            EnterValue("content textbox", text);
            SwitchDefaultFrame();
        }
    }
}