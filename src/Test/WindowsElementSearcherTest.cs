using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class WindowsElementSearcherTest {
        [TestInitialize]
        public void Initialize() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            TestProcessHelper.LaunchProcess(TestProcessHelper.ProcessType.Calculator);
        }

        [TestCleanup]
        public void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
        }

        [TestMethod]
        public void CanFindCalculator() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("window", "Calculator");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("group", "Standard functions");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create("button", "Square root"));
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create("button", "Cube"));
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual(4, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Calculator);
            log = new List<string>();
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNull(element);
            Assert.AreEqual(0, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec);
            Assert.IsNull(element);
        }
    }
}
