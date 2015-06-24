using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models;
using System.Collections.Generic;
using Detector.Models.Others;
using Detector.Models.ORM;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;
using Detector.Extractors.LINQToSQL40;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class RoslynORMModelTreeExtractorTests
    {
        RoslynORMModelTreeExtractor target;
        List<DatabaseEntityDeclaration<LINQToSQL>> dbEntityDeclarations;

        [TestMethod]
        [Ignore]
        public async Task ExtractsORMModelTree_When_MethodHasADbAccessingMethodCallStatement()
        {
            //Arrange
            var solGen = new RoslynComplexSolutionGenerator();
            var solution = solGen.GetRoslynSolution();

            SyntaxNode rootNodeCustomerRepository
               = await solGen.GetRootNodeForCustomerRepositoryClassDocument();

            IEnumerable<MethodDeclarationSyntax> methodDeclarations = rootNodeCustomerRepository.DescendantNodes().OfType<MethodDeclarationSyntax>();

            SemanticModel model = await solGen.GetSemanticModelForCustomerRepositoryClass();

            await ExtractEntityDeclarations(solution);

            var dbQueryExtractor = new LINQToSQLDatabaseQueryExtractor(model, dbEntityDeclarations);
            dbQueryExtractor.Visit(rootNodeCustomerRepository);
            var databaseQueries = dbQueryExtractor.DatabaseQueries;

            target = new RoslynORMModelTreeExtractor(databaseQueries);
            //Act            
            ORMModelTree result = target.Extract(methodDeclarations.First());

            //Assert
            Assert.IsTrue(result.RootNode.Model is MethodDeclaration);
            Assert.IsTrue((result.RootNode as MethodDeclaration).MethodName == "GetCustomer");

            Assert.IsTrue(result.RootNode.ChildNodes[0] is DatabaseAccessingMethodCallStatement<LINQToSQL>);
        }

    
        private async Task ExtractEntityDeclarations(Solution solution)
        {
            dbEntityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();
           
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await document.GetSyntaxRootAsync();

                    var dbEntityDeclarationExtractor = new LINQToSQLDatabaseEntityDeclarationExtractor();
                    dbEntityDeclarationExtractor.Visit(root);
                    var result = dbEntityDeclarationExtractor.DatabaseEntityDeclarations;
                    if (result.Count > 0)
                    {
                        dbEntityDeclarations.AddRange(result);
                    }
                 }
            }
        }
    }
}
