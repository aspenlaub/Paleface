using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface ITextBox {
        string AutomationId { get; }
        string ClassName { get; }
        string Name { get; }

        // ReSharper disable once UnusedMemberInSuper.Global
        AppiumWebElement EditableElement { get; set; }
        void Clear();
        void CopyToClipboard();
        void PasteFromClipboard();
        string Text { get; set; }

        void SetText(string s, bool useCopyPaste);
    }
}