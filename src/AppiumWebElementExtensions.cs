using System.Collections.Generic;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public static class AppiumWebElementExtensions {
        public static string GetName(this AppiumWebElement element) {
            return element.GetAttribute("Name");
        }

        public static string GetLocalizedControlType(this AppiumWebElement element) {
            return element.GetAttribute("LocalizedControlType");
        }

        public static IReadOnlyCollection<AppiumWebElement> FindElementsByWindowsElementSearchSpec(this AppiumWebElement element, WindowsElementSearchSpec windowsElementSearchSpec) {
            return element.FindElementsByXPath(windowsElementSearchSpec.XPath());
        }
    }
}
