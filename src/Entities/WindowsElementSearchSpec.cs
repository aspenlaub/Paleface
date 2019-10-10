using System.Collections.Generic;
using System.Linq;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Extensions;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Entities {
    public class WindowsElementSearchSpec {
        public List<string> ClassNames { get; set; } = new List<string>();
        public string Name { get; set; }
        public string NameContains { get; set; }
        public List<string> NameDoesNotContain { get; set; } = new List<string>();
        public bool NameMustNotBeEmpty { get; set; }

        public List<WindowsElementSearchSpec> WindowsChildElementSearchSpecs { get; } = new List<WindowsElementSearchSpec>();

        public WindowsElementSearchSpec() {
        }

        public WindowsElementSearchSpec(AppiumWebElement element) {
            ClassNames = new List<string> { element.GetClassName() };
            Name = element.GetName();
        }

        public static WindowsElementSearchSpec Create(string className, string name) {
            return new WindowsElementSearchSpec { ClassNames = string.IsNullOrWhiteSpace(className) ? new List<string>() : new List<string> { className }, Name = name };
        }

        public override string ToString() {
            var enquote = false;
            var formattedClassNames = string.Join("or ", ClassNames.Select(n => $"\"{n}\""));
            var s = string.IsNullOrWhiteSpace(Name) ? $"Unnamed {formattedClassNames}" : $"\"{Name}\" of class {formattedClassNames}";
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
