using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleWindowResources = Aspenlaub.Net.GitHub.CSharp.Paleface.SampleWindow.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class SampleWindowTest {
        protected static readonly string SampleWindowExecutable = typeof(SampleWindowTest).Assembly.Location
            .Replace(@"\Test\", @"\SampleWindow\")
            .Replace("Aspenlaub.Net.GitHub.CSharp.Paleface.Test.dll", @"Aspenlaub.Net.GitHub.CSharp.Paleface.SampleWindow.exe");
        protected static readonly string SampleWindowTitle = SampleWindowResources.WindowTitle;

        [TestMethod]
        public void CanUseTextBox() {
            using var sut = new WindowsElement(SampleWindowExecutable, SampleWindowTitle, () => {});

            var textBox = sut.FindTextBox("SampleTextBox");
            Assert.IsNotNull(textBox);
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
            using var sut = new WindowsElement(SampleWindowExecutable, SampleWindowTitle, () => { });

            var comboBox = sut.FindComboBox("SampleComboBox");
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
