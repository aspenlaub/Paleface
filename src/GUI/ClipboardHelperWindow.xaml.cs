using System;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.GUI {
    /// <summary>
    /// Interaction logic for ClipboardHelperWindow.xaml
    /// </summary>
    public partial class ClipboardHelperWindow {
        public ClipboardHelperWindow() {
            InitializeComponent();
        }

        private void ClipboardHelperWindow_OnActivated(object sender, EventArgs e) {
            Title = Properties.Resources.WindowTitle;
        }
    }
}
