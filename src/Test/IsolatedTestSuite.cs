using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Paleface.Test {
    public class IsolatedTestSuite {
        private static readonly object SyncRoot = new object();

        [TestInitialize]
        public void Initialize() {
            Monitor.Enter(SyncRoot);
        }

        [TestCleanup]
        public void Cleanup() {
            Monitor.Exit(SyncRoot);
        }
    }
}
