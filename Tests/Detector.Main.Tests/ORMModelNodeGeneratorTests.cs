using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;

namespace Detector.Main.Tests
{
    [TestClass]
    public class ORMModelNodeGeneratorTests
    {
        ORMModelNodeGenerator<LINQToSQL> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMModelNodeGenerator<LINQToSQL>();
        }

        [TestMethod]
        public void GeneratesModelNodeWithDatabaseQuery_When_ModelIsTypeDatabaseQuery()
        {
            //Arrange
            DatabaseQuery<LINQToSQL> dbQuery = new DatabaseQuery<LINQToSQL>("", null);

            //Act
            target.Visit(dbQuery);

            //Assert
        }
    }
}
