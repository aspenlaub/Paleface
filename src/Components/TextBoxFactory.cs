using Aspenlaub.Net.GitHub.CSharp.Paleface.GUI;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Components {
    public class TextBoxFactory : ITextBoxFactory {
        private readonly IStringPreparator vStringPreparator;

        public TextBoxFactory(IStringPreparator stringPreparator) {
            vStringPreparator = stringPreparator;
        }

        public ITextBox Create(AppiumWebElement editableElement) {
            return new TextBox(vStringPreparator) {
                EditableElement = editableElement
            };
        }
    }
}
