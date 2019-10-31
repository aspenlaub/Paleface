using System.Collections.Generic;
using OpenQA.Selenium.Appium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces {
    public interface ITextBoxServices {
        ITextBox Create(AppiumWebElement editableElement);
        bool FindClipboardHelperWindowEnterTextAndCopy(string textToCopy, out IList<string> log);
    }
}
