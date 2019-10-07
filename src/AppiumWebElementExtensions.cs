using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public static class AppiumWebElementExtensions {
        public static string GetName(this AppiumWebElement element) {
            return element.GetAttribute("Name");
        }

        public static string GetAutomationId(this AppiumWebElement element) {
            return element.GetAttribute("AutomationId");
        }

        public static string GetClassName(this AppiumWebElement element) {
            return element.GetAttribute("ClassName");
        }
    }
}
