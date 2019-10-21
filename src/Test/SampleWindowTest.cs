using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Components;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleWindowResources = Aspenlaub.Net.GitHub.CSharp.Paleface.SampleWindow.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class SampleWindowTest : IsolatedTestSuite {
        protected static readonly string SampleWindowExecutable = typeof(SampleWindowTest).Assembly.Location
            .Replace(@"\Test\", @"\SampleWindow\")
            .Replace("Aspenlaub.Net.GitHub.CSharp.Paleface.Test.dll", @"Aspenlaub.Net.GitHub.CSharp.Paleface.SampleWindow.exe");

        protected static readonly string SampleWindowTitle = SampleWindowResources.WindowTitle;

        private readonly IContainer vContainer;

        public SampleWindowTest() {
            var builder = new ContainerBuilder().UsePaleface();
            vContainer = builder.Build();
        }

        [TestMethod]
        public void CanUseTextBox() {
            using var sut = vContainer.Resolve<IAppiumSession>();
            sut.Initialize(SampleWindowExecutable, SampleWindowTitle, () => {}, 2);

            var textBox = sut.FindTextBox("SampleTextBoxAutoId");
            Assert.IsNotNull(textBox);
            Assert.AreEqual("SampleTextBoxAutoId", textBox.AutomationId);
            Assert.AreEqual(UiClassNames.TextBox, textBox.ClassName);
            Assert.AreEqual("SampleTextBoxAutoName", textBox.Name);
            textBox.Clear();
            Assert.AreEqual(string.Empty, textBox.Text);
            const string text = @"Works with a simple text, the words 'yes' and 'zero'";
            textBox.Text = text;
            Assert.AreEqual(text, textBox.Text);
            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            textBox.Text = fileName;
            Assert.AreEqual(fileName, textBox.Text);
        }

        [TestMethod]
        public void CanUseComboBox() {
            using var sut = vContainer.Resolve<IAppiumSession>();
            sut.Initialize(SampleWindowExecutable, SampleWindowTitle, () => { }, 2);

            var comboBox = sut.FindComboBox("SampleComboBoxAutoId", out var comboBoxElement);
            Assert.IsNotNull(comboBox);
            Assert.AreEqual("SampleComboBoxAutoId", comboBoxElement.GetAutomationId());
            Assert.AreEqual(UiClassNames.ComboBox, comboBoxElement.GetClassName());
            Assert.AreEqual("SampleComboBoxAutoName", comboBoxElement.GetName());

            comboBox = sut.FindComboBox("SampleComboBoxAutoId");
            Assert.IsNotNull(comboBox);
            comboBox.Clear();
            Assert.AreEqual(string.Empty, comboBox.Text);
            const string text = @"Works with a simple text, the words 'yes' and 'zero'";
            comboBox.Text = text;
            Assert.AreEqual(text, comboBox.Text);
            var fileName = Path.GetTempPath() + @"\TextFile.txt";
            comboBox.Text = fileName;
            Assert.AreEqual(fileName, comboBox.Text);
        }
    }
}
