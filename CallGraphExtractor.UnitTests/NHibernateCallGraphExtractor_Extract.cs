using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CallGraphExtractor.UnitTests
{
    [TestClass]
    public class NHibernateCallGraphExtractor_Extract
    {
        NHibernateCallGraphExtractor_Ctor target;

        [TestInitialize]
        public void Initialize()
        {
            target = new NHibernateCallGraphExtractor_Ctor();
        }

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
