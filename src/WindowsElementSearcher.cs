﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using AppiumWindowsElement = OpenQA.Selenium.Appium.Windows.WindowsElement;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElementSearcher {
        public AppiumWindowsElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec) {
            return SearchWindowsElement(windowsElementSearchSpec, new List<string>());
        }

        public AppiumWindowsElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec, List<string> log) {
            AppiumHelper.LaunchWinAppDriverIfNecessary();

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");

            WindowsDriver<AppiumWindowsElement> desktopSession;
            try {
                desktopSession = new WindowsDriver<AppiumWindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            } catch (WebDriverException) {
                throw new Exception("WinAppDriver.exe process could not be contacted");
            }

            log.Clear();
            var xpath = windowsElementSearchSpec.XPath();
            var result = desktopSession.FindElementsByXPath(xpath).FirstOrDefault(e => DoesElementMatchSearchSpec(e, windowsElementSearchSpec, 0, log));
            return result;
        }

        private static bool DoesElementMatchSearchSpec(AppiumWebElement element, WindowsElementSearchSpec windowsElementSearchSpec, int depth, ICollection<string> log) {
            if (element.GetAttribute("LocalizedControlType") != windowsElementSearchSpec.LocalizedControlType) { return false; }
            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.Name) && element.GetAttribute("Name") != windowsElementSearchSpec.Name) { return false; }

            if (!windowsElementSearchSpec.WindowsChildElementSearchSpecs.All(
                    spec => element.FindElementsByXPath(spec.XPath()).Any(e => DoesElementMatchSearchSpec(e, spec, depth + 1, log))
                )) {
                    return false;
            }

            log.Add($"Found '{windowsElementSearchSpec.Name}' of type '{windowsElementSearchSpec.LocalizedControlType}' at depth {depth}");
            return true;
        }
    }
}
