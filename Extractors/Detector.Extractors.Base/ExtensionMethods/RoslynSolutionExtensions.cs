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
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
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

        public static async Task<Dictionary<ClassDeclarationSyntax, SemanticModel>> GetClassesOfType(this Solution solution, string typeName)
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
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                    {
                        if (classDeclarationSyntax.AttributeLists.ToString().Contains(typeof(T).ToString()))
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
