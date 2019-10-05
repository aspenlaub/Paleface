using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class ControlWithEditableText {
        protected AppiumWebElement EditableElement;

        public string Text {
            get => EditableElement.Text;
            set {
                Clear();
                EditableElement.SendKeys(StringPreparator.PrepareStringForInput(value));

            }
        }

        public ControlWithEditableText(AppiumWebElement editableElement) {
            EditableElement = editableElement;
        }

        public void Clear() {
            EditableElement.SendKeys(Keys.Control + "a" + Keys.Control);
            EditableElement.SendKeys(Keys.Delete);
        }
    }
}
