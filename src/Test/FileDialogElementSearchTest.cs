using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class FileDialogElementSearchTest {
        [TestInitialize]
        public void Initialize() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
            TestProcessHelper.LaunchProcess(TestProcessHelper.ProcessType.WordPad);
        }

        [TestCleanup]
        public void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
        }

        [TestMethod]
        public void CanUseFileDialog() {
            var sut = new WindowsElementSearcher();

            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("window", "");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("document", "Rich Text Window");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            var log = new List<string>();
            AppiumWebElement element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element, "Wordpad document not found");

            element.SendKeys(Keys.Control + 'o' + Keys.Control);

            windowsElementSearchSpec = WindowsElementSearchSpec.Create("window", "");
            windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("dialog", "Open");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            element = sut.SearchWindowsElement(windowsElementSearchSpec);
            Assert.IsNotNull(element, "File dialog not found");

            element = element.FindElementsByWindowsElementSearchSpec(new WindowsElementSearchSpec { Name = "File name:", LocalizedControlType = "edit" }).FirstOrDefault();
            Assert.IsNotNull(element, "File name not found");

            var comboTextBox = new TextBox(element);
            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            comboTextBox.Text = fileName;
            Assert.AreEqual(fileName, comboTextBox.Text);
        }
    }
}
