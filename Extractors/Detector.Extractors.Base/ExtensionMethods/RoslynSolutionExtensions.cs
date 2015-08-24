using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.Base.ExtensionMethods
{
    public static class RoslynSolutionExtensions
    {
        public static async Task<Dictionary<ClassDeclarationSyntax, SemanticModel>> GetClassesOfType<T>(this Solution solution)
        {
            int totalNumber = GetTotalNumberOfDocuments(solution);
            string type = typeof(T).ToString();
            // progress.Report(new ExtractionProgress(string.Format("Finding classes of type {0}", type)));

            int counter = 0;
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    counter++;
                    //progress.Report(GetExtractionProgress(totalNumber, counter));

                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                    {
                        if (semanticModel.IsOfType<T>(classDeclarationSyntax))
                        {
                            result.Add(classDeclarationSyntax, semanticModel);
                        }
                    }
                }
            }

            return result;
        }

        static int _totalNumberOfDocuments;
        private static int GetTotalNumberOfDocuments(Solution solution)
        {
            if (_totalNumberOfDocuments == 0)
            {
                int counter = 0;
                foreach (var project in solution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        counter++;
                    }
                }
                _totalNumberOfDocuments = counter;
            }
            return _totalNumberOfDocuments;
        }

        private static ExtractionProgress GetExtractionProgress(int totalNumber, int counter)
        {
            return new ExtractionProgress(counter * 100 / totalNumber);
        }

        public static async Task<Dictionary<ClassDeclarationSyntax, SemanticModel>> GetClassesOfType(this Solution solution, string typeName)
        {
            int totalNumber = GetTotalNumberOfDocuments(solution);
            // progress.Report(new ExtractionProgress(string.Format("Finding classes of type {0}", typeName)));

            int counter = 0;
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    counter++;
                    //  progress.Report(GetExtractionProgress(totalNumber, counter));

                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                    {
                        if (semanticModel.IsOfType(classDeclarationSyntax, typeName))
                        {
                            result.Add(classDeclarationSyntax, semanticModel);
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<Dictionary<ClassDeclarationSyntax, SemanticModel>> GetClassesSignedWithAttributeType<T>(this Solution solution)
        {
            return await solution.GetClassesSignedWithAttributeType(typeof(T).ToString());
        }

        public static async Task<Dictionary<ClassDeclarationSyntax, SemanticModel>> GetClassesSignedWithAttributeType(this Solution solution, string typeName)
        {
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                    {
                        if (classDeclarationSyntax.AttributeLists.ToString().Contains(typeName))
                        {
                            result.Add(classDeclarationSyntax, semanticModel);
                        }
                    }
                }
            }

            return result;
        }
    }
}
