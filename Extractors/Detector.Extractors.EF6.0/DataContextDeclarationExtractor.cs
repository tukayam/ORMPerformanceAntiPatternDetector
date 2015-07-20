using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.EF60
{
    public class DataContextDeclarationExtractor : DataContextDeclarationExtractor<Detector.Models.ORM.EntityFramework>
    {
        private ModelCollection<DataContextDeclaration<EntityFramework>> _dataContextDeclarations;
        public ModelCollection<DataContextDeclaration<EntityFramework>> DataContextDeclarations
        {
            get
            {
                return _dataContextDeclarations;
            }
        }

        public DataContextDeclarationExtractor()
        {
            _dataContextDeclarations = new ModelCollection<DataContextDeclaration<EntityFramework>>();
        }

        public async Task FindDataContextDeclarationsAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                await FindDataContextDeclarationsAsync(project);
            }
        }

        public async Task FindDataContextDeclarationsAsync(Project project)
        {
            foreach (var document in project.Documents)
            {
                SyntaxNode root = await document.GetSyntaxRootAsync();
                SemanticModel semanticModel = await document.GetSemanticModelAsync();
               
                foreach (ClassDeclarationSyntax classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    ISymbol symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    
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
