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

            log.Clear();
            var xpath = windowsElementSearchSpec.XPath();
            var elements = desktopSession.FindElementsByXPath(xpath).ToList();
            var reverseSearchSpecs = elements.Select(e => new WindowsElementSearchSpec(e));
            log.Add(elements.Any()
                ? $"XPath {windowsElementSearchSpec.XPath()} applied to root element resulted in {elements.Count} elements: {string.Join(", ", reverseSearchSpecs)}"
                : $"XPath {windowsElementSearchSpec.XPath()} applied to root element did not yield any results");

            foreach (var useDepthSearch in new[] { false, true }) {
                foreach (var element in elements) {
                    if (!DoesElementMatchSearchSpec(element, windowsElementSearchSpec, 0, log, useDepthSearch, out var elementOrMatchingChildElement)) {
                        continue;
                    }

                    if (elementOrMatchingChildElement == null) {
                        throw new Exception("Element matches search specification, but no element is returned");
                    }
                    return elementOrMatchingChildElement;
                }
            }
            return null;
        }

        private static bool DoesElementMatchSearchSpec(AppiumWebElement element, WindowsElementSearchSpec windowsElementSearchSpec, int depth, ICollection<string> log, bool useDepthSearch, out AppiumWebElement elementOrMatchingChildElement) {
            elementOrMatchingChildElement = element;
            if (depth > 20) {
                throw new Exception($"Search depth exceeds {depth}");
            }

            var reverseSearchSpec = new WindowsElementSearchSpec(element);
            log.Add($"Checking {windowsElementSearchSpec} against {reverseSearchSpec} at depth {depth}");
            if (element.GetLocalizedControlType() != windowsElementSearchSpec.LocalizedControlType) {
                log.Add($"Mismatch, localized control type is {element.GetLocalizedControlType()}");
                return false;
            }

            if (windowsElementSearchSpec.NameMustNotBeEmpty) {
                if (string.IsNullOrWhiteSpace(element.GetName())) {
                    if (useDepthSearch) {
                        log.Add("No immediate match, name must not be empty, checking child elements");
                        if (DoesAnyChildElementMatchSearchSpec(element, windowsElementSearchSpec, depth, log, out var potentialElementOrMatchingChildElement)) {
                            elementOrMatchingChildElement = potentialElementOrMatchingChildElement;
                            if (elementOrMatchingChildElement == null) {
                                throw new Exception("Child element matches search specification, but no element is returned");
                            }

                            log.Add($"Found '{elementOrMatchingChildElement.GetName()}' of type '{elementOrMatchingChildElement.GetLocalizedControlType()}' at depth {depth}");
                            return true;
                        }
                    }

                    log.Add("Mismatch, name must not be empty");
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.Name) && element.GetName() != windowsElementSearchSpec.Name) {
                log.Add($"Mismatch, name is {element.GetName()}");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.NameContains)) {
                var elementName = element.GetName();
                if (elementName?.Contains(windowsElementSearchSpec.NameContains) != true) {
                    if (useDepthSearch) {
                        log.Add($"No immediate match, name {element.GetName() ?? "''"} does not contain {windowsElementSearchSpec.NameContains}, checking child elements");
                        if (DoesAnyChildElementMatchSearchSpec(element, windowsElementSearchSpec, depth, log, out var potentialElementOrMatchingChildElement)) {
                            elementOrMatchingChildElement = potentialElementOrMatchingChildElement;
                            if (elementOrMatchingChildElement == null) {
                                throw new Exception("Child element matches search specification, but no element is returned");
                            }

                            log.Add($"Found '{elementOrMatchingChildElement.GetName()}' of type '{elementOrMatchingChildElement.GetLocalizedControlType()}' at depth {depth}");
                            return true;
                        }
                    }

                    log.Add($"Mismatch, name {element.GetName()} does not contain {windowsElementSearchSpec.NameContains}");
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(windowsElementSearchSpec.NameDoesNotContain)) {
                var elementName = element.GetName();
                if (elementName?.Contains(windowsElementSearchSpec.NameDoesNotContain) == true) {
                    log.Add($"No immediate match, name {element.GetName()} contains {windowsElementSearchSpec.NameDoesNotContain}, checking child elements");
                    if (useDepthSearch) {
                        if (DoesAnyChildElementMatchSearchSpec(element, windowsElementSearchSpec, depth, log, out var potentialElementOrMatchingChildElement)) {
                            elementOrMatchingChildElement = potentialElementOrMatchingChildElement;
                            if (elementOrMatchingChildElement == null) {
                                throw new Exception("Child element matches search specification, but no element is returned");
                            }

                            log.Add($"Found '{elementOrMatchingChildElement.GetName()}' of type '{elementOrMatchingChildElement.GetLocalizedControlType()}' at depth {depth}");
                            return true;
                        }
                    }

                    log.Add($"Mismatch, name {element.GetName()} contains {windowsElementSearchSpec.NameDoesNotContain}");
                    return false;
                }
            }

            log.Add($"Element is indeed {windowsElementSearchSpec}, checking child search specifications");
            if (!windowsElementSearchSpec.WindowsChildElementSearchSpecs.All(
                    spec => {
                        var elements = element.FindElementsByXPath(spec.XPath()).ToList();
                        if (!elements.Any()) {
                            log.Add($"XPath {spec.XPath()} applied to element {windowsElementSearchSpec} did not yield any results");
                        }
                        return elements.Any(e => DoesElementMatchSearchSpec(e, spec, depth + 1, log, useDepthSearch, out _));
                    })) {

                log.Add($"Child specifications not met for '{elementOrMatchingChildElement.GetName()}' of type '{elementOrMatchingChildElement.GetLocalizedControlType()}' at depth {depth}");
                return false;
            }

            if (elementOrMatchingChildElement == null) {
                throw new Exception("Child element matches search specification, but no element is returned");
            }
            log.Add($"Found '{elementOrMatchingChildElement.GetName()}' of type '{elementOrMatchingChildElement.GetLocalizedControlType()}' at depth {depth}");
            return true;
        }

        private static bool DoesAnyChildElementMatchSearchSpec(AppiumWebElement element, WindowsElementSearchSpec windowsElementSearchSpec, int depth, ICollection<string> log, out AppiumWebElement childElement) {
            foreach(var e in element.FindElementsByXPath(windowsElementSearchSpec.XPath(element.Id))) {
                if (DoesElementMatchSearchSpec(e, windowsElementSearchSpec, depth + 1, log, true, out childElement)) {
                    return true;
                }
            }

            childElement = null;
            return false;
        }
    }
}
