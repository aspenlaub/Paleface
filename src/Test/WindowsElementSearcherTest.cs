using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    [TestClass]
    public class WindowsElementSearcherTest {
        [TestInitialize]
        public void Initialize() {
            ShutDownRunningCalculators();
            LaunchCalculator();
        }

        [TestCleanup]
        public void Cleanup() {
            ShutDownRunningCalculators();
        }

        [TestMethod]
        public void CanFindCalculator() {
            var windowsElementSearchSpec = WindowsElementSearchSpec.Create("window", "Calculator");
            var windowsChildElementSearchSpec = WindowsElementSearchSpec.Create("group", "Standard functions");
            windowsElementSearchSpec.WindowsChildElementSearchSpecs.Add(windowsChildElementSearchSpec);
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create("button", "Square root"));
            windowsChildElementSearchSpec.WindowsChildElementSearchSpecs.Add(WindowsElementSearchSpec.Create("button", "Cube"));
            var sut = new WindowsElementSearcher();
            var log = new List<string>();
            var element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            Assert.AreEqual(4, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNotNull(element);
            ShutDownRunningCalculators();
            log = new List<string>();
            element = sut.SearchWindowsElement(windowsElementSearchSpec, log);
            Assert.IsNull(element);
            Assert.AreEqual(0, log.Count);
            element = sut.SearchWindowsElement(windowsElementSearchSpec);
            Assert.IsNull(element);
        }

        private static void ShutDownRunningCalculators() {
            var processes = Process.GetProcessesByName("calculator").ToList();
            if (!processes.Any()) {
                return;
            }

            foreach (var process in processes) {
                process.Kill();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            processes = Process.GetProcessesByName("calculator").ToList();
            if (!processes.Any()) {
                return;
            }

            throw new Exception("Could not close all file calculators");
        }

        private static void LaunchCalculator() {
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "calc.exe",
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    WorkingDirectory = Environment.SystemDirectory
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName("calculator").Length != 1) {
                throw new Exception("File calculator process could not be started");
            }
        }
    }
}
