using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Entities;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface IWindowsElementSearcher {
        AppiumWebElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec);
        AppiumWebElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec, IList<string> log);
        AppiumWebElement SearchWindowsElement(AppiumWebElement parentElement, WindowsElementSearchSpec windowsElementSearchSpec, IList<string> log);
    }
}