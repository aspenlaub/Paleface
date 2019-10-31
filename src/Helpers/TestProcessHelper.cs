using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Paleface.GUI;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Helpers {
    public static class TestProcessHelper {
        public enum ProcessType {
            Calculator,
            WordPad,
            Paleface
        }

        private static readonly string ClipboardHelperWindowExecutable = typeof(ClipboardHelperWindow).Assembly.Location
            .Replace(@"\Test\", @"\")
            .Replace(".dll", @".exe");

        private static string ProcessName(ProcessType processType) {
            return processType == ProcessType.Paleface
                ? "Aspenlaub.Net.GitHub.CSharp.Paleface"
                : Enum.GetName(typeof(ProcessType), processType)?.ToLower();
        }

        public static void ShutDownRunningProcesses(ProcessType processType) {
            var processName = ProcessName(processType);
            var processes = Process.GetProcessesByName(processName).ToList();
            if (!processes.Any()) {
                return;
            }

            foreach (var process in processes) {
                process.Kill();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            processes = Process.GetProcessesByName(processName).ToList();
            if (!processes.Any()) {
                return;
            }

            throw new Exception($"Could not close all {Enum.GetName(typeof(ProcessType), processType)} processes");
        }

        public static void LaunchProcess(ProcessType processType) {
            string executable;
            switch (processType) {
                case ProcessType.Calculator:
                    executable = "calc.exe";
                    break;
                case ProcessType.WordPad:
                    executable = "wordpad.exe";
                    break;
                case ProcessType.Paleface:
                    executable = ClipboardHelperWindowExecutable;
                    break;
                default:
                    throw  new NotImplementedException();
            }

            AppiumHelper.LaunchWinAppDriverIfNecessary();

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", executable);
            options.AddAdditionalCapability("unicodeKeyboard", true);
            options.AddAdditionalCapability("platform", "Windows");

            try {
                var _ = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            } catch (WebDriverException) {
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (!IsProcessRunning(processType)) {
                throw new Exception($"{Enum.GetName(typeof(ProcessType), processType)} process could not be started");
            }
        }

        public static bool IsProcessRunning(ProcessType processType) {
            var processName = ProcessName(processType);
            return Process.GetProcessesByName(processName).Length == 1;
        }
    }
}
