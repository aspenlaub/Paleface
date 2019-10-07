﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class CalculatorElementSearchTest {
        [ClassInitialize]
        public static void Initialize(TestContext context) {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            TestProcessHelper.LaunchProcess(TestProcessHelper.ProcessType.Calculator);
        }

        [ClassCleanup]
        public static void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
        }

        [TestMethod]
        public void CanFindCalculator() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.ApplicationFrameWindow, "Calculator");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.NamedContainerAutomationPeer, "Standard functions");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create(UiClassNames.Button, "Square root"));
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create(UiClassNames.Button, "Cube"));
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual("Calculator", element.GetName());
            Assert.AreEqual(UiClassNames.ApplicationFrameWindow, element.GetClassName());
            Assert.AreEqual(16, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void CannotFindShutDownCalculator() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.ApplicationFrameWindow, "Calculator");
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNull(element);
            Assert.AreEqual(1, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec);
            Assert.IsNull(element);
            TestProcessHelper.LaunchProcess(TestProcessHelper.ProcessType.Calculator);
        }

        [TestMethod]
        public void CanUseOptionalSearchCriteria() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("", "Desktop 1");
            windowsElementSearchSpec.NameMustNotBeEmpty = true;
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual("Desktop 1", element.GetName());
            Assert.AreEqual("#32769", element.GetClassName());
            var elementName = element.GetName();
            Assert.IsFalse(string.IsNullOrWhiteSpace(elementName));
            Assert.IsTrue(elementName.Contains("Desktop"));
        }

        [TestMethod]
        public void CanUseNameDoesNotContainCriteria() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.Button, "");
            windowsElementSearchSpec.NameMustNotBeEmpty = true;
            windowsElementSearchSpec.NameDoesNotContain = new List<string> { "User Notifications Indicator", "Open Navigation" , "Keep on top", "Minimize", "Restore", "Close" };
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.IsFalse(string.IsNullOrWhiteSpace(element.GetName()));
            Assert.IsFalse(windowsElementSearchSpec.NameDoesNotContain.Any(n => element.GetName().Contains(n)));
        }

        [TestMethod]
        public void CanUseNameContainsCriteria() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.ApplicationFrameWindow, "Calculator");
            windowsElementSearchSpec.NameContains = "Calculator";
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual("Calculator", element.GetName());
            Assert.AreEqual(UiClassNames.ApplicationFrameWindow, element.GetClassName());
            Assert.AreEqual("Calculator", element.GetName());
        }
    }
}
