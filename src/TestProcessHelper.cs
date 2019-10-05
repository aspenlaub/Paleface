using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = executable,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    WorkingDirectory = Environment.SystemDirectory
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName(processName).Length != 1) {
                throw new Exception($"{Enum.GetName(typeof(ProcessType), processType)} process could not be started");
            }
        }

    }
}
