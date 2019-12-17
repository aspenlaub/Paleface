using Aspenlaub.Net.GitHub.CSharp.Paleface.Extensions;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    public class ControlWithSingleEditableElement {
        public AppiumWebElement EditableElement { get; set; }

        public string AutomationId => EditableElement.GetAutomationId();
        public string ClassName => EditableElement.GetClassName();
        public string Name => EditableElement.GetName();
    }
}
