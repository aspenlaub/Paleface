using System.Collections.Generic;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface {
    public class WindowsElementSearchSpec {
        public string LocalizedControlType { get; set; }
        public string Name { get; set; }
        public string NameContains { get; set; }
        public string NameDoesNotContain { get; set; }
        public bool NameMustNotBeEmpty { get; set; }

        public List<WindowsElementSearchSpec> WindowsChildElementSearchSpecs { get; }

        public WindowsElementSearchSpec() {
            WindowsChildElementSearchSpecs = new List<WindowsElementSearchSpec>();
        }

        public WindowsElementSearchSpec(AppiumWebElement element) {
            LocalizedControlType = element.GetLocalizedControlType();
            Name = element.GetName();
        }

        public static WindowsElementSearchSpec Create(string localizedControlType, string name) {
            return new WindowsElementSearchSpec {
                LocalizedControlType = localizedControlType, Name = name
            };
        }

        public override string ToString() {
            var enquote = false;
            var s = string.IsNullOrWhiteSpace(Name) ? $"Unnamed {LocalizedControlType}" : $"{Name} of type {LocalizedControlType}";
            if (!string.IsNullOrWhiteSpace(NameContains)) {
                enquote = true;
                s += $" with name containing {NameContains}";
            }
            if (!string.IsNullOrWhiteSpace(NameDoesNotContain)) {
                enquote = true;
                s += $" with name not containing {NameDoesNotContain}";
            } 
            if (NameMustNotBeEmpty) {
                enquote = true;
                s += " with non-empty name";
            }
            return enquote ? $"\"{s}\"" : s;
        }

        public string XPath(string parentId) {
            var conditions = XPathConditions();
            // conditions.Add("not(#id='" + parentId + "')"); does not work
            return "./*//*[" + string.Join(" and ", conditions) + ']';
        }

        public string XPath() {
            var conditions = XPathConditions();
            return "//*[" + string.Join(" and ", conditions) + ']';
        }

        private List<string> XPathConditions() {
            var conditions = new List<string> {
                "@LocalizedControlType='" + LocalizedControlType + "'"
            };
            if (!string.IsNullOrWhiteSpace(Name)) {
                conditions.Add("@Name='" + Name + "'");
            }

            return conditions;
        }
    }
}
