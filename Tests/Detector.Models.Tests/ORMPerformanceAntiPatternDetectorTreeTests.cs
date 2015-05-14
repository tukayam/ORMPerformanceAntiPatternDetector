using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models;

namespace Detector.Extractors.Tests.Models
{
    [TestClass]
    public class ORMPerformanceAntiPatternDetectorTreeTests
    {
        ORMPerformanceAntiPatternDetectorTree target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMPerformanceAntiPatternDetectorTree();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
