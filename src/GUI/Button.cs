using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    public class Button : ControlWithSingleEditableElement, IButton {
        public void Click() {
            EditableElement.SendKeys("");
            EditableElement.SendKeys(Keys.Enter);
        }
    }
}