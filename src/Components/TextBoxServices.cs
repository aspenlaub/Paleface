using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Entities;
using Aspenlaub.Net.GitHub.CSharp.Paleface.GUI;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Components {
    public class TextBoxServices : ITextBoxServices {
        private readonly IStringPreparator vStringPreparator;
        private readonly IWindowsElementSearcher vWindowsElementSearcher;

        public TextBoxServices(IStringPreparator stringPreparator, IWindowsElementSearcher windowsElementSearcher) {
            vStringPreparator = stringPreparator;
            vWindowsElementSearcher = windowsElementSearcher;
        }

        public ITextBox Create(AppiumWebElement editableElement) {
            return new TextBox(vStringPreparator, this) {
                EditableElement = editableElement
            };
        }

        public bool FindClipboardHelperWindowEnterTextAndCopy(string textToCopy, out IList<string> log) {
            log = new List<string>();
            if (!TestProcessHelper.IsProcessRunning(TestProcessHelper.ProcessType.Paleface)) {
                log.Add("No paleface process could be found");
                return false;
            }

            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("", Properties.Resources.WindowTitle);
            var element = vWindowsElementSearcher.SearchWindowsElement(windowsElementSearchSpec, log);
            if (element == null) { return false; }

            windowsElementSearchSpec = WindowsElementSearchSpec.Create("", "TextToCopyAutoId");
            element = vWindowsElementSearcher.SearchWindowsElement(element, windowsElementSearchSpec, log);
            if (element == null) { return false; }

            var textBox = Create(element);
            textBox.Clear();
            textBox.SetText(textToCopy, false);
            textBox.CopyToClipboard();
            textBox.PasteFromClipboard();
            return textBox.Text == textToCopy;
        }
    }
}
