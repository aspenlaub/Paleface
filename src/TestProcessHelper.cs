using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class TestProcessHelper {
        public enum ProcessType {
            Calculator,
            WordPad
        }

        private static string ProcessName(ProcessType processType) {
            return Enum.GetName(typeof(ProcessType), processType)?.ToLower();
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
            var processName = ProcessName(processType);
            string executable;
            switch (processType) {
                case ProcessType.Calculator:
                    executable = "calc.exe";
                    break;
                case ProcessType.WordPad:
                    executable = "wordpad.exe";
                    break;
                default:
                    throw  new NotImplementedException();
            }

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", executable);
            options.AddAdditionalCapability("unicodeKeyboard", true);
            options.AddAdditionalCapability("platform", "Windows");

            try {
                var _ = new WindowsDriver<OpenQA.Selenium.Appium.Windows.WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            } catch (WebDriverException) {
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName(processName).Length != 1) {
                throw new Exception($"{Enum.GetName(typeof(ProcessType), processType)} process could not be started");
            }
        }
    }
}
