using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface IControlWithSingleEditableElement {
        string AutomationId { get; }
        string ClassName { get; }
        string Name { get; }

        // ReSharper disable once UnusedMemberInSuper.Global
        AppiumWebElement EditableElement { get; set; }
    }
}
