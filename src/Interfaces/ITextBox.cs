using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface ITextBox {
        string AutomationId { get; }
        string ClassName { get; }
        string Name { get; }

        AppiumWebElement EditableElement { get; set; }
        void Clear();
        string Text { get; set; }
    }
}