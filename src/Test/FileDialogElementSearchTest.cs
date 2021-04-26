using System.Collections.Generic;
using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Components;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Entities;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class FileDialogElementSearchTest : IsolatedTestSuite {
        private readonly IContainer vContainer;

        public FileDialogElementSearchTest() {
            var builder = new ContainerBuilder().UsePaleface();
            vContainer = builder.Build();
        }

        [TestInitialize]
        public new void Initialize() {
            base.Initialize();
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
            TestProcessHelper.LaunchProcessAsync(TestProcessHelper.ProcessType.WordPad).Wait();
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Paleface);
            TestProcessHelper.LaunchProcessAsync(TestProcessHelper.ProcessType.Paleface).Wait();
        }

        [TestCleanup]
        public new void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.WordPad);
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Paleface);
            base.Cleanup();
        }

        [TestMethod]
        public void CanUseFileDialog() {
            var sut = vContainer.Resolve<IWindowsElementSearcher>();

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

            var comboBox = vContainer.Resolve<ITextBoxServices>().Create(element);

            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            comboBox.Text = fileName;
            Assert.AreEqual(fileName, comboBox.Text);
        }
    }
}
