using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class TextBox : ControlWithEditableText {
        public TextBox(AppiumWebElement editBoxElement) : base(editBoxElement) {
        }
    }
}
