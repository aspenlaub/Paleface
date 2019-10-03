using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using AppiumWindowsElement = OpenQA.Selenium.Appium.Windows.WindowsElement;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElement : IDisposable {
        public WindowsDriver<AppiumWindowsElement> Session { get; private set; }

        protected Action DismissPromptsOnClosing;

        public WindowsElement(string executableOrRunningApplication, string expectedTitle, Action dismissPromptsOnClosing) {
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

        public TextBox FindTextBox(string accessibleName) {
            return new TextBox(Session.FindElementByAccessibilityId(accessibleName));
        }
    }
}
