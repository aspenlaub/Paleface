using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class TextBox {
        protected AppiumWebElement EditBox;

        public string Text {
            get => EditBox.Text;
            set => EditBox.SendKeys(StringPreparator.PrepareStringForInput(value));
        }

        public TextBox(AppiumWebElement editBox) {
            EditBox = editBox;
        }

        public void Clear() {
            EditBox.SendKeys(Keys.Control + "a" + Keys.Control);
            EditBox.SendKeys(Keys.Delete);
        }
    }
}
