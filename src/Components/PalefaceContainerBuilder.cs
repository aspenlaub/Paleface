using Aspenlaub.Net.GitHub.CSharp.Paleface.GUI;
using Aspenlaub.Net.GitHub.CSharp.Paleface.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Components {
    public static class PalefaceContainerBuilder {
        public static ContainerBuilder UsePaleface(this ContainerBuilder builder) {
            builder.RegisterType<AppiumSession>().As<IAppiumSession>();
            builder.RegisterType<StringPreparator>().As<IStringPreparator>();
            builder.RegisterType<TextBoxFactory>().As<ITextBoxFactory>();
            builder.RegisterType<WindowsElementSearcher>().As<IWindowsElementSearcher>();
            return builder;
        }
    }
}
