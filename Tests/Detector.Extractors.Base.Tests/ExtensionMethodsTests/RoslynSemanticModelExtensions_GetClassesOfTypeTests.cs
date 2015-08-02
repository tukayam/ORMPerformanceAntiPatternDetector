using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase.RoslynSolutionGenerators;
using Detector.Extractors.Base.ExtensionMethods;
using EF60_NW;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Data.Entity;
using TestBase.Stubs;

namespace Detector.Extractors.Base.Tests.ExtensionMethodsTests
{
    [TestClass]
    public class RoslynSemanticModelExtensions_GetClassesOfTypeTests
    {
        Solution EF60_NWSolution;
        ProgressStub progressIndicator;

        [TestInitialize]
        public void Initialize()
        {
            progressIndicator = new ProgressStub();
        }

        [TestMethod]
        public async Task FindClasses_When_GenericMethodIsCalledAndTypeIsClassTypeItself()
        {
            //Arrange
            var solution = await GetEF60_NWSolution();

            //Act
            Dictionary<ClassDeclarationSyntax, SemanticModel> result
                = await solution.GetClassesOfType<Order>();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Keys.First().Identifier.ToString() == "Order");
            Assert.IsNotNull(result[result.Keys.First()]);
        }

        [TestMethod]
        public async Task FindClasses_When_GenericMethodIsCalledAndTypeIsBaseType()
        {
            //Arrange  
            var solution = await GetEF60_NWSolution();

            //Act
            Dictionary<ClassDeclarationSyntax, SemanticModel> result
                = await solution.GetClassesOfType<DbContext>();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Keys.First().Identifier.ToString() == "NWDbContext");
            Assert.IsNotNull(result[result.Keys.First()]);
        }

        [TestMethod]
        public async Task FindClasses_When_GenericMethodIsCalledAndTypeIsInterface()
        {
            //Arrange  
            var solution = await GetEF60_NWSolution();

            //Act
            Dictionary<ClassDeclarationSyntax, SemanticModel> result = await solution.GetClassesOfType<IRepository>();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Keys.First().Identifier.ToString() == "Repository");
            Assert.IsNotNull(result[result.Keys.First()]);
        }

        [TestMethod]
        public async Task FindClasses_When_NonGenericMethodIsCalledAndTypeIsInterface()
        {
            //Arrange  
            var solution = await GetEF60_NWSolution();

            //Act
            Dictionary<ClassDeclarationSyntax, SemanticModel> result = await solution.GetClassesOfType("IRepository");

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Keys.First().Identifier.ToString() == "Repository");
            Assert.IsNotNull(result[result.Keys.First()]);
        }

        [TestMethod]
        public async Task FindClasses_When_NonGenericMethodIsCalledAndTypeIsBaseType()
        {
            //Arrange  
            var solution = await GetEF60_NWSolution();

            //Act
            Dictionary<ClassDeclarationSyntax, SemanticModel> result = await solution.GetClassesOfType("DbContext");

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Keys.First().Identifier.ToString() == "NWDbContext");
            Assert.IsNotNull(result[result.Keys.First()]);
        }

        private async Task<Solution> GetEF60_NWSolution()
        {
            if (EF60_NWSolution == null)
            {
                EF60_NWSolution =
                    await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");
            }
            return EF60_NWSolution;
        }
    }
}
