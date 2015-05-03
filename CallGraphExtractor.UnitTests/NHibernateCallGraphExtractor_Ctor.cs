using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CallGraphExtractor.UnitTests
{
    [TestClass]
    public class NHibernateCallGraphExtractor_Ctor
    {
        NHibernateCallGraphExtractor target;

        [TestMethod]
        public void DerivesFromCallGraphExtractor()
        {
            //Arrange
            //Act
            target = new NHibernateCallGraphExtractor();

            //Assert
            Assert.IsTrue(target is CallGraphExtractor);
        }      
    }
}
