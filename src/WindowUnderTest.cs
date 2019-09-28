using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowUnderTest : IDisposable {
        public WindowsDriver<WindowsElement> Session { get; private set; }

        protected Action DismissPromptsOnClosing;

        public WindowUnderTest(string executable, string expectedTitle, Action dismissPromptsOnClosing) {
            var options = new AppiumOptions();
            if (!File.Exists(executable)) {
                throw new FileNotFoundException($"File does not exist: {executable}");
            }
            options.AddAdditionalCapability("app", executable);
            options.AddAdditionalCapability("unicodeKeyboard", true);
            options.AddAdditionalCapability("platform", "Windows");

            LaunchWinAppDriverIfNecessary();
            try {
                Session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
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

        private static void LaunchWinAppDriverIfNecessary() {
            if (Process.GetProcessesByName("WinAppDriver").Length != 0) {
                return;
            }

            var winAppDriverExecutable = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Windows Application Driver\WinAppDriver.exe";
            if (!File.Exists(winAppDriverExecutable)) {
                throw new Exception("WinAppDriver.exe process could not found. Remember that it must be installed from https://github.com/Microsoft/WinAppDriver/releases/");
            }

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = winAppDriverExecutable
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName("WinAppDriver").Length != 1) {
                throw new Exception("WinAppDriver.exe process could not be started. Make sure Windows is in Developer Mode");
            }
        }

        public TextBox FindTextBox(string accessibleName) {
            return new TextBox(Session.FindElementByAccessibilityId(accessibleName));
        }
    }
}
