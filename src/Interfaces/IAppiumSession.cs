using System;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface IAppiumSession : IDisposable {
        void Initialize(string executableOrRunningApplication, string expectedTitle, Action dismissPromptsOnClosing, int implicitSecondsUntilTimeOut);
        ITextBox FindTextBox(string accessibleName);
        ITextBox FindComboBox(string accessibleName);
        ITextBox FindComboBox(string accessibleName, out AppiumWebElement comboBoxElement);
    }
}