using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Paleface.GUI;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
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
            ShutDownRunningProcesses(ProcessName(processType));
        }

        public static void ShutDownRunningProcesses(string processName) {
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

            throw new Exception($"Could not close all {processName} processes");
        }

        public static void LaunchProcess(ProcessType processType) {
            LaunchProcess(Executable(processType), ProcessName(processType));
        }

        public static void LaunchProcess(string executable, string processName) {
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
            if (!IsProcessRunning(processName)) {
                throw new Exception($"{processName} process could not be started");
            }
        }

        private static string Executable(ProcessType processType) {
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
                    if (!File.Exists(executable)) {
                        var container = new ContainerBuilder().UsePegh(new DummyCsArgumentPrompter()).Build();
                        const string folderName = @"$(GitHub)\PalefaceBin\Release";
                        var errorsAndInfos = new ErrorsAndInfos();
                        var folder = container.Resolve<IFolderResolver>().Resolve(folderName, errorsAndInfos);
                        if (!errorsAndInfos.AnyErrors()) {
                            executable = folder.FullName + @"\Aspenlaub.Net.GitHub.CSharp.Paleface.exe";
                            if (!File.Exists(executable)) {
                                throw new Exception($"{Enum.GetName(typeof(ProcessType), processType)} process could not be started");
                            }
                        }
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }

            return executable;
        }

        public static bool IsProcessRunning(ProcessType processType) {
            return IsProcessRunning(ProcessName(processType));
        }

        public static bool IsProcessRunning(string processName) {
            return Process.GetProcessesByName(processName).Length == 1;
        }
    }
}
