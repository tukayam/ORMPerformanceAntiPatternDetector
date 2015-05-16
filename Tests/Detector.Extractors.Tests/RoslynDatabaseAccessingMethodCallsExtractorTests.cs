using Detector.Extractors.Tests.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class RoslynDatabaseAccessingMethodCallsExtractorTests
    {
        RoslynDatabaseAccessingMethodCallsExtractor target;

        [TestMethod]
        public void ExtractsDatabaseAccessingMethodCall_When_CallIsDoneOnAnIQueryableWithLINQToSQL()
        {
            //Arrange

            string text = @"class C
{
public string name;
}
class Program
{
    private static int i = 0;
    public static void Main()
    {
        //int j = 0; j += i;
        C obj=new C();
        // What symbols are in scope here?
    }
}";

            ProjectId projectId = ProjectId.CreateNewId();
            DocumentId documentId = DocumentId.CreateNewId(projectId);

            var solution = new AdhocWorkspace().CurrentSolution
                .AddProject(projectId, "MyProject", "MyProject", LanguageNames.CSharp)
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(object).Assembly))
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.Linq.DataContext).Assembly))
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.DataTable).Assembly))
                .AddDocument(documentId, "MyFile.cs", text);

            var document = solution.GetDocument(documentId);
            var model = document.GetSemanticModelAsync().Result;


            target = new RoslynDatabaseAccessingMethodCallsExtractor(model);

            //Act
            target.Visit(document.GetSyntaxRootAsync().Result);

            //Assert
            Assert.IsTrue(target.DatabaseAccessingMethodCalls.Count() == 1);
            Assert.IsTrue(target.DatabaseAccessingMethodCalls.ToList()[0].DatabaseEntityObject.DatabaseEntityDeclaration.Name == "Employee");
        }
    }
}
