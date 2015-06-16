using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using Detector.Models;
using System.Collections.Generic;
using Detector.Models.Others;
using Detector.Models.ORM;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Detector.Extractors.DatabaseEntities;
using System.Threading.Tasks;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class ORMModelTreeExtractorTests
    {
        ORMModelTreeExtractor target;
        List<DatabaseEntityDeclaration<LINQToSQL>> dbEntityDeclarations;

        [TestMethod]
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

            target = new ORMModelTreeExtractor(databaseQueries);
            //Act            
            ORMModelTree result = target.Extract(methodDeclarations.First());

            //Assert
            Assert.IsTrue(result.RootNode.Model is MethodDeclaration);
            Assert.IsTrue((result.RootNode as MethodDeclaration).MethodName == "GetCustomer");

            Assert.IsTrue(result.RootNode.ChildNodes[0] is DatabaseAccessingMethodCallStatement<LINQToSQL>);
        }

        [TestMethod]
        public async Task ExtractsORMModelTree_When_RoslynSimpleSolutionGeneratorIsUsed()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";


            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel model = await solGenerator.GetSemanticModelForMainClass();
            SyntaxNode rootNode =await solGenerator.GetRootNodeForMainDocument();
            MethodDeclarationSyntax methodDeclarationSyntaxNode = rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var solution = solGenerator.GetRoslynSolution(textToPlaceInMainMethod);
            await ExtractEntityDeclarations(solution);

            var dbQueryExtractor = new LINQToSQLDatabaseQueryExtractor(model, dbEntityDeclarations);
            dbQueryExtractor.Visit(rootNode);
            var databaseQueries = dbQueryExtractor.DatabaseQueries;

            target = new ORMModelTreeExtractor(databaseQueries);
            //Act            
            ORMModelTree result = target.Extract(methodDeclarationSyntaxNode);

            //Assert
            Assert.IsTrue(result.RootNode.Model is MethodDeclaration);
            Assert.IsTrue((result.RootNode.Model as MethodDeclaration).MethodName == "GetEmployeeById");

            Assert.IsTrue(result.RootNode.ChildNodes[0].Model is DataContextDeclaration<LINQToSQL>);
            Assert.IsTrue(result.RootNode.ChildNodes[1].Model is DatabaseAccessingMethodCallStatement<LINQToSQL>);
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
