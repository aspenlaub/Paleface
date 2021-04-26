using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Components;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Entities;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class CalculatorElementSearchTest : IsolatedTestSuite {
        private readonly IContainer vContainer;

        public CalculatorElementSearchTest() {
            var builder = new ContainerBuilder().UsePaleface();
            vContainer = builder.Build();
        }

        [TestInitialize]
        public new void Initialize() {
            base.Initialize();
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            TestProcessHelper.LaunchProcessAsync(TestProcessHelper.ProcessType.Calculator).Wait();
        }

        [TestCleanup]
        public new void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            base.Cleanup();
        }

        [TestMethod]
        public void CanFindCalculator() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.ApplicationFrameWindow, "Calculator");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.NamedContainerAutomationPeer, "Standard functions");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create(UiClassNames.Button, "Square root"));
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create(UiClassNames.Button, "Reciprocal"));
            var sut = vContainer.Resolve<IWindowsElementSearcher>();
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
        public async Task CannotFindShutDownCalculator() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create(UiClassNames.ApplicationFrameWindow, "Calculator");
            var sut = vContainer.Resolve<IWindowsElementSearcher>();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNull(element);
            Assert.AreEqual(1, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec);
            Assert.IsNull(element);
            await TestProcessHelper.LaunchProcessAsync(TestProcessHelper.ProcessType.Calculator);
        }

        [TestMethod]
        public void CanUseOptionalSearchCriteria() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("", "Desktop 1");
            windowsElementSearchSpec.NameMustNotBeEmpty = true;
            var sut = vContainer.Resolve<IWindowsElementSearcher>();
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
            var sut = vContainer.Resolve<IWindowsElementSearcher>();
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
            var sut = vContainer.Resolve<IWindowsElementSearcher>();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual("Calculator", element.GetName());
            Assert.AreEqual(UiClassNames.ApplicationFrameWindow, element.GetClassName());
            Assert.AreEqual("Calculator", element.GetName());
        }
    }
}
