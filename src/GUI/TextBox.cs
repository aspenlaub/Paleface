using System;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    public class TextBox : ControlWithSingleEditableElement, ITextBox {
        protected readonly IStringPreparator StringPreparator;
        protected readonly ITextBoxServices TextBoxServices;

        public TextBox(IStringPreparator stringPreparator, ITextBoxServices textBoxServices) {
            StringPreparator = stringPreparator;
            TextBoxServices = textBoxServices;
        }

        public string Text {
            get => EditableElement.Text;
            set => SetText(value, true);
        }

        public void SetText(string s, bool useCopyPaste) {
            if (useCopyPaste) {
                if (!TextBoxServices.FindClipboardHelperWindowEnterTextAndCopy(s, out var log)) {
                    throw new Exception($"Failed to enter text and copy\r\n{string.Join("\r\n", log)}");
                }
                PasteFromClipboard();
            } else {
                Clear();
                EditableElement.SendKeys(StringPreparator.PrepareStringForInput(s));
            }
        }

        public void Clear() {
            EditableElement.SendKeys("");
            EditableElement.SendKeys(Keys.Control + "a" + Keys.Control);
            EditableElement.SendKeys(Keys.Delete);
        }

        public void CopyToClipboard() {
            EditableElement.SendKeys("");
            EditableElement.SendKeys(Keys.Control + "a" + Keys.Control);
            EditableElement.SendKeys(Keys.Control + "c" + Keys.Control);
        }

        public void PasteFromClipboard() {
            Clear();
            EditableElement.SendKeys(Keys.Control + "v" + Keys.Control);
        }
    }
}