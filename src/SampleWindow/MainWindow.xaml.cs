using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.SampleWindow {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            Title = Properties.Resources.WindowTitle;
        }

        private void SampleSayHelloWorldButton_OnClick(object sender, RoutedEventArgs e) {
            SampleTextBox.Text = SampleTextBox.Text == "Hello World" ? "I Already Said Hello" : "Hello World";
        }
    }
}
