using System.Collections.Generic;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElementSearchSpec {
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string NameContains { get; set; }
        public List<string> NameDoesNotContain { get; set; } = new List<string>();
        public bool NameMustNotBeEmpty { get; set; }

        public List<WindowsElementSearchSpec> WindowsChildElementSearchSpecs { get; } = new List<WindowsElementSearchSpec>();

        public WindowsElementSearchSpec() {
        }

        public WindowsElementSearchSpec(AppiumWebElement element) {
            ClassName = element.GetClassName();
            Name = element.GetName();
        }

        public static WindowsElementSearchSpec Create(string className, string name) {
            return new WindowsElementSearchSpec { ClassName = className, Name = name };
        }

        public override string ToString() {
            var enquote = false;
            var s = string.IsNullOrWhiteSpace(Name) ? $"Unnamed {ClassName}" : $"\"{Name}\" of class \"{ClassName}\"";
            if (!string.IsNullOrWhiteSpace(NameContains)) {
                enquote = true;
                s += $" with name containing \"{NameContains}\"";
            }
            foreach (var nameDoesNotContain in NameDoesNotContain) {
                enquote = true;
                s += $" with name not containing \"{nameDoesNotContain}\"";
            }
            if (NameMustNotBeEmpty) {
                s += " with non-empty name";
            }
            return enquote || NameMustNotBeEmpty ? $"\"{s}\"" : s;
        }
    }
}
