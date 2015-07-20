using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase;
using Microsoft.CodeAnalysis;

namespace Detector.Extractors.EF602.Tests
{
    [TestClass]
    public class DatabaseAccessingMethodCallExtractorTests
    {
        //[TestMethod]
        //[TestCategory("IntegrationTest")]
        //public async Task DetectsDbAccessingMethodCalls_When_EF60_NWProjectIsCompiled()
        //{
        //    //Arrange  
        //    Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");
        //    var target = new DatabaseAccessingMethodCallExtractor();

        //    //Act
        //    await target.FindDataContextDeclarationsAsync(EF60_NWSolution);

        //    //Assert
        //    Assert.IsTrue(target.DataContextDeclarations.Count == 1);

        //    foreach (var item in target.DataContextDeclarations)
        //    {
        //        Assert.IsTrue(item.Name == "NWDbContext");
        //    }
        //}
    }
}