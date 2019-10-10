using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface ITextBoxFactory {
        ITextBox Create(AppiumWebElement editableElement);
    }
}
