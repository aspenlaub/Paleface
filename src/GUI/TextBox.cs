using Aspenlaub.Net.GitHub.CSharp.Paleface.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    public class TextBox : ITextBox {
        public AppiumWebElement EditableElement { get; set; }

        protected readonly IStringPreparator StringPreparator;

        public TextBox(IStringPreparator stringPreparator) {
            StringPreparator = stringPreparator;
        }

        public string AutomationId => EditableElement.GetAutomationId();
        public string ClassName => EditableElement.GetClassName();
        public string Name => EditableElement.GetName();

        public string Text {
            get => EditableElement.Text;
            set {
                Clear();
                EditableElement.SendKeys(StringPreparator.PrepareStringForInput(value));
            }
        }

        public void Clear() {
            EditableElement.SendKeys(Keys.Control + "a" + Keys.Control);
            EditableElement.SendKeys(Keys.Delete);
        }
    }
}