using System;
using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using AppiumWindowsElement = OpenQA.Selenium.Appium.Windows.WindowsElement;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    public class AppiumSession : IAppiumSession {
        public WindowsDriver<AppiumWindowsElement> Session { get; private set; }

        protected readonly IStringPreparator StringPreparator;
        protected readonly ITextBoxFactory TextBoxFactory;

        protected Action DismissPromptsOnClosing;

        public AppiumSession(IStringPreparator stringPreparator, ITextBoxFactory textBoxFactory) {
            StringPreparator = stringPreparator;
            TextBoxFactory = textBoxFactory;
        }

        public void Initialize(string executableOrRunningApplication, string expectedTitle, Action dismissPromptsOnClosing) {
            AppiumHelper.LaunchWinAppDriverIfNecessary();

            var options = new AppiumOptions();
            if (!File.Exists(executableOrRunningApplication)) {
                throw new FileNotFoundException($"File does not exist: {executableOrRunningApplication}");
            }
            options.AddAdditionalCapability("app", executableOrRunningApplication);
            options.AddAdditionalCapability("unicodeKeyboard", true);
            options.AddAdditionalCapability("platform", "Windows");

            try {
                Session = new WindowsDriver<AppiumWindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            } catch (WebDriverException) {
                throw new Exception("WinAppDriver.exe process could not be contacted");
            }

            if (Session?.SessionId == null) {
                throw new Exception("WinAppDriver.exe process could not be contacted");
            }

            if (Session.Title != expectedTitle) {
                throw new Exception($"Title '{expectedTitle}' was expected, but it is '{Session.Title}'");
            }
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            DismissPromptsOnClosing = dismissPromptsOnClosing;
        }

        public void Dispose() {
            if (Session == null) {
                return;
            }

            Session.Close();

            DismissPromptsOnClosing();

            Session.Quit();
            Session = null;
        }

        public ITextBox FindTextBox(string accessibleName) {
            if (Session == null) {
                throw new Exception($"Please call method {nameof(Initialize)}, first");
            }

            return TextBoxFactory.Create(Session.FindElementByAccessibilityId(accessibleName));
        }

        public ITextBox FindComboBox(string accessibleName) {
            if (Session == null) {
                throw new Exception($"Please call method {nameof(Initialize)}, first");
            }

            return FindComboBox(accessibleName, out _);
        }

        public ITextBox FindComboBox(string accessibleName, out AppiumWebElement comboBoxElement) {
            if (Session == null) {
                throw new Exception($"Please call method {nameof(Initialize)}, first");
            }

            comboBoxElement = Session.FindElementByAccessibilityId(accessibleName);
            return TextBoxFactory.Create(comboBoxElement.FindElementByClassName(UiClassNames.TextBox));
        }
    }
}
