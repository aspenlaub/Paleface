using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Components;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class ClipboardHelperWindowTest : IsolatedTestSuite {
        private readonly IContainer vContainer;

        public ClipboardHelperWindowTest() {
            var builder = new ContainerBuilder().UsePaleface();
            vContainer = builder.Build();
        }

        [TestInitialize]
        public new void Initialize() {
            base.Initialize();
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Paleface);
            TestProcessHelper.LaunchProcessAsync(TestProcessHelper.ProcessType.Paleface).Wait();
        }

        [TestCleanup]
        public new void Cleanup() {
            TestProcessHelper.ShutDownRunningProcesses(TestProcessHelper.ProcessType.Paleface);
            base.Cleanup();
        }

        [TestMethod]
        public void CanUseClipboardHelperWindow() {
            var textBoxServices = vContainer.Resolve<ITextBoxServices>();
            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            Assert.IsTrue(textBoxServices.FindClipboardHelperWindowEnterTextAndCopy(fileName, out var log), $"Action failed\r\n{string.Join("\r\n", log)}");
        }
    }
}
