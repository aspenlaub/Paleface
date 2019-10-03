using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    internal static class AppiumHelper {
        internal static void LaunchWinAppDriverIfNecessary() {
            if (Process.GetProcessesByName("WinAppDriver").Length != 0) {
                return;
            }

            var winAppDriverExecutable = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Windows Application Driver\WinAppDriver.exe";
            if (!File.Exists(winAppDriverExecutable)) {
                throw new Exception("WinAppDriver.exe process could not found. Remember that it must be installed from https://github.com/Microsoft/WinAppDriver/releases/");
            }

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = winAppDriverExecutable,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(winAppDriverExecutable) ?? ""
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName("WinAppDriver").Length != 1) {
                throw new Exception("WinAppDriver.exe process could not be started. Make sure Windows is in Developer Mode");
            }
        }
    }
}
