using Detector.Extractors.Base;
using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.LINQToSQL40
{
    public  class DataContextDeclarationExtractor : DataContextDeclarationExtractor<LINQToSQL>
    {
        public DataContextDeclarationExtractor(Context<LINQToSQL> context)
            :base(context)
        {  }

        public override async Task ExtractDataContextDeclarationsAsync(Project project)
        {
            foreach (var document in project.Documents)
            {
                SyntaxNode root = await document.GetSyntaxRootAsync();
                SemanticModel semanticModel = await document.GetSemanticModelAsync();

                foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    if (classDeclarationSyntax.AttributeLists.ToString().Contains("DatabaseAttribute"))
                    {
                        DataContextDeclarations.Add(new DataContextDeclaration<LINQToSQL>(classDeclarationSyntax.Identifier.ToString(), classDeclarationSyntax.GetCompilationInfo()));
                    }
                }
            }
        }
    }
}
