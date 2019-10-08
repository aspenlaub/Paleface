using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class FileDialogElementSearchTest : IsolatedTestSuite {
        [TestInitialize]
        public new void Initialize() {
            base.Initialize();
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
            TestProcessHelper.LaunchProcess(TestProcessHelper.ProcessType.WordPad);
        }

        [TestCleanup]
        public new void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
            base.Cleanup();
        }

        [TestMethod]
        public void CanUseFileDialog() {
            var sut = new WindowsElementSearcher();

            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("", "Document - WordPad");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("", "Rich Text Window");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element, $"Wordpad document not found\r\n{string.Join("\r\n", log)}");

            element.SendKeys(Keys.Control + 'o' + Keys.Control);

            windowsElementSearchSpec = WindowsElementSearchSpec.Create("", "Document - WordPad");
            windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("", "Open");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element, $"File dialog not found\r\n{string.Join("\r\n", log)}");

            element = sut.SearchWindowsElement(element, WindowsElementSearchSpec.Create(UiClassNames.Edit , "File name:"), log);
            Assert.IsNotNull(element, $"File name not found\r\n{string.Join("\r\n", log)}");

            var comboTextBox = new TextBox(element);
            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            comboTextBox.Text = fileName;
            Assert.AreEqual(fileName, comboTextBox.Text);
        }
    }
}
