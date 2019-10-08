using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using AppiumWindowsElement = OpenQA.Selenium.Appium.Windows.WindowsElement;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElementSearcher {
        public AppiumWebElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec) {
            return SearchWindowsElement(windowsElementSearchSpec, new List<string>());
        }

        public AppiumWebElement SearchWindowsElement(WindowsElementSearchSpec windowsElementSearchSpec, List<string> log) {
            AppiumHelper.LaunchWinAppDriverIfNecessary();

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");

            WindowsDriver<AppiumWindowsElement> desktopSession;
            try {
                desktopSession = new WindowsDriver<AppiumWindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            } catch (WebDriverException) {
                throw new Exception("WinAppDriver.exe process could not be contacted");
            }

            return SearchWindowsElement(desktopSession, null, windowsElementSearchSpec, log);
        }

        public AppiumWebElement SearchWindowsElement(AppiumWebElement parentElement, WindowsElementSearchSpec windowsElementSearchSpec, List<string> log) {
            return SearchWindowsElement(null, parentElement, windowsElementSearchSpec, log);
        }

        protected AppiumWebElement SearchWindowsElement(WindowsDriver<AppiumWindowsElement> desktopSession, AppiumWebElement parentElement, WindowsElementSearchSpec windowsElementSearchSpec, List<string> log) {
            log.Clear();
            var elements = new List<AppiumWebElement>();
            if (desktopSession != null && !string.IsNullOrWhiteSpace(windowsElementSearchSpec.Name)) {
                elements = desktopSession.FindElementsByName(windowsElementSearchSpec.Name).Cast<AppiumWebElement>().ToList();
            } else if (desktopSession != null && windowsElementSearchSpec.ClassNames.Any()) {
                windowsElementSearchSpec.ClassNames.ForEach(n => elements.AddRange(desktopSession.FindElementsByClassName(n)));
            } else if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.Name)) {
                elements = parentElement.FindElementsByName(windowsElementSearchSpec.Name).ToList();
            } else if (windowsElementSearchSpec.ClassNames.Any()) {
                windowsElementSearchSpec.ClassNames.ForEach(n => elements.AddRange(parentElement.FindElementsByClassName(n)));
            } else {
                throw new ArgumentException($"Invalid {nameof(WindowsElementSearchSpec)}, name or class name is required");
            }
            var reverseSearchSpecs = elements.Select(e => new WindowsElementSearchSpec(e));
            log.Add(elements.Any()
                ? $"[{windowsElementSearchSpec}] applied to root element resulted in {elements.Count} elements: {string.Join(", ", reverseSearchSpecs)}"
                : $"[{windowsElementSearchSpec}] applied to root element did not yield any results");

            foreach (var element in elements) {
                if (!DoesElementMatchSearchSpec(element, windowsElementSearchSpec, 0, log, "", out var elementOrMatchingChildElement)) {
                    continue;
                }

                if (elementOrMatchingChildElement == null) {
                    throw new Exception("Element matches search specification, but no element is returned");
                }
                return elementOrMatchingChildElement;
            }
            return null;
        }

        private static bool DoesElementMatchSearchSpec(AppiumWebElement element, WindowsElementSearchSpec windowsElementSearchSpec, int depth, ICollection<string> log, string parentElementIds, out AppiumWebElement elementOrMatchingChildElement) {
            if (parentElementIds.Contains(element.Id + ';')) {
                throw new Exception($"Element {element.Id} already is a parent element");
            }

            parentElementIds += element.Id + ';';

            elementOrMatchingChildElement = element;
            if (depth > 20) {
                throw new Exception($"Search depth exceeds {depth}");
            }

            var reverseSearchSpec = new WindowsElementSearchSpec(element);
            log.Add($"Checking {windowsElementSearchSpec} against {reverseSearchSpec} at depth {depth}");
            if (windowsElementSearchSpec.ClassNames.Any() && !windowsElementSearchSpec.ClassNames.Contains(element.GetClassName())) {
                log.Add($"Mismatch, class name is {element.GetClassName() ?? "empty"}");
                return false;
            }

            if (windowsElementSearchSpec.NameMustNotBeEmpty) {
                if (string.IsNullOrWhiteSpace(element.GetName())) {
                    log.Add("Mismatch, name must not be empty");
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.Name) && element.GetName() != windowsElementSearchSpec.Name) {
                log.Add($"Mismatch, name is {element.GetName() ?? "empty"}");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.NameContains)) {
                var elementName = element.GetName();
                if (elementName?.Contains(windowsElementSearchSpec.NameContains) != true) {
                    log.Add($"Mismatch, name {element.GetName() ?? "empty"} does not contain {windowsElementSearchSpec.NameContains}");
                    return false;
                }
            }

            foreach (var nameDoesNotContain in windowsElementSearchSpec.NameDoesNotContain.Where(n => element.GetName()?.Contains(n) == true)) {
                log.Add($"Mismatch, name {element.GetName()} contains {nameDoesNotContain}");
                return false;
            }

            log.Add($"Element is indeed {windowsElementSearchSpec}, checking child search specifications");
            if (!windowsElementSearchSpec.WindowsChildElementSearchSpecs.All(
                    spec => {
                        var elements = new List<AppiumWebElement>();
                        if (!string.IsNullOrWhiteSpace(spec.Name)) {
                            elements = element.FindElementsByName(spec.Name).ToList();
                        } else if (spec.ClassNames.Any()) {
                            spec.ClassNames.ForEach(n => elements.AddRange(element.FindElementsByClassName(n)));
                        } else {
                            throw new ArgumentException($"Invalid {nameof(WindowsElementSearchSpec)}, name or class name is required");
                        }
                        var reverseSearchSpecs = elements.Select(e => new WindowsElementSearchSpec(e)).ToList();
                        log.Add(elements.Any()
                            ? $"[{spec}] applied to parent element {new WindowsElementSearchSpec(element)} resulted in {elements.Count} elements: {string.Join(", ", reverseSearchSpecs)}"
                            : $"[{spec}] applied to parent element {new WindowsElementSearchSpec(element)} did not yield any results");
                        return elements.Any(e => DoesElementMatchSearchSpec(e, spec, depth + 1, log, parentElementIds, out _));
                    })) {

                log.Add($"Child specifications not met for '{elementOrMatchingChildElement.GetName()}' of class '{elementOrMatchingChildElement.GetClassName()}' at depth {depth}");
                return false;
            }

            if (elementOrMatchingChildElement == null) {
                throw new Exception("Child element matches search specification, but no element is returned");
            }
            log.Add($"Found '{elementOrMatchingChildElement.GetName()}' of class '{elementOrMatchingChildElement.GetClassName()}' at depth {depth}");
            return true;
        }
    }
}
