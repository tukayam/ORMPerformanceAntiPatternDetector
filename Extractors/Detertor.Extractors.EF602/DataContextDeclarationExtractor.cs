using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Detector.Extractors.Base;

namespace Detector.Extractors.EF602
{
    public class DataContextDeclarationExtractor : DataContextDeclarationExtractor<Detector.Models.ORM.EntityFramework>
    {
        public DataContextDeclarationExtractor(Context<EntityFramework> context)
            :base(context)
        {

        }

        public override async Task ExtractDataContextDeclarationsAsync(Project project)
        {
            foreach (var document in project.Documents)
            {
                SyntaxNode root = await document.GetSyntaxRootAsync();
                SemanticModel semanticModel = await document.GetSemanticModelAsync();

                foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    INamedTypeSymbol symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    if (InheritsFrom<DbContext>(symbol))
                    {
                        DataContextDeclarations.Add(new DataContextDeclaration<EntityFramework>(classDeclarationSyntax.Identifier.ToString(), classDeclarationSyntax.GetCompilationInfo()));
                    }
                }
            }
        }

        private bool InheritsFrom<T>(INamedTypeSymbol symbol)
        {
            while (true)
            {
                if (symbol.ToString() == typeof(T).FullName)
                {
                    return true;
                }
                if (symbol.BaseType != null)
                {
                    symbol = symbol.BaseType;
                    continue;
                }
                break;
            }
            return false;
        }
    }
}
