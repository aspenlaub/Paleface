namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface ITextBox : IControlWithSingleEditableElement {
        void Clear();
        void CopyToClipboard();
        void PasteFromClipboard();
        string Text { get; set; }

        void SetText(string s, bool useCopyPaste);
    }
}