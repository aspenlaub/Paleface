using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElementSearchSpec {
        public string LocalizedControlType { get; set; }
        public string Name { get; set; }

        public List<WindowsElementSearchSpec> WindowsChildElementSearchSpecs { get; }

        public WindowsElementSearchSpec() {
            WindowsChildElementSearchSpecs = new List<WindowsElementSearchSpec>();
        }

        public static WindowsElementSearchSpec Create(string localizedControlType, string name) {
            return new WindowsElementSearchSpec {
                LocalizedControlType = localizedControlType, Name = name
            };
        }

        public override string ToString() {
            return string.IsNullOrWhiteSpace(Name) ? $"Unnamed {LocalizedControlType}" : $"{Name} of type {LocalizedControlType}";
        }

        public string XPath() {
            var xpath = "//*[@LocalizedControlType='" + LocalizedControlType + "']";
            if (!string.IsNullOrWhiteSpace(Name)) {
                xpath = xpath + "[@Name='" + Name + "']";
            }

            return xpath;
        }
    }
}
