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
    }
}
