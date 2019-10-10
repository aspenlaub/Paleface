using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using OpenQA.Selenium;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Components {
    public class StringPreparator : IStringPreparator {

        private const string CharactersNotRequiringPreparation = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXabcdefghijklmnopqrstuvwx ";

        public string PrepareStringForInput(string s) {
            var r = "";
            foreach (var c in s) {
                if (CharactersNotRequiringPreparation.Contains(c.ToString())) {
                    r += c;
                    continue;
                }

                r += Keys.Alt;
                var code = ((int)c).ToString();
                foreach (var digit in code) {
                    switch (digit) {
                        case '0':
                            r += Keys.NumberPad0;
                            break;
                        case '1':
                            r += Keys.NumberPad1;
                            break;
                        case '2':
                            r += Keys.NumberPad2;
                            break;
                        case '3':
                            r += Keys.NumberPad3;
                            break;
                        case '4':
                            r += Keys.NumberPad4;
                            break;
                        case '5':
                            r += Keys.NumberPad5;
                            break;
                        case '6':
                            r += Keys.NumberPad6;
                            break;
                        case '7':
                            r += Keys.NumberPad7;
                            break;
                        case '8':
                            r += Keys.NumberPad8;
                            break;
                        case '9':
                            r += Keys.NumberPad9;
                            break;
                    }
                }
                r += Keys.Alt;
            }
            return r;
        }
    }
}
