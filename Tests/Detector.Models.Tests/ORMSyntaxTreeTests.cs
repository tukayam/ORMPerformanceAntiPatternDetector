using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models;

namespace Detector.Extractors.Tests.Models
{
    [TestClass]
    public class ORMPerformanceAntiPatternDetectorTreeTests
    {
        ORMSyntaxTree target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMSyntaxTree();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
