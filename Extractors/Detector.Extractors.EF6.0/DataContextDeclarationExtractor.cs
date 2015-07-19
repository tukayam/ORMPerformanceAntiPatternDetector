using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;
using Detector.Extractors.Base.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System;

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

        SemanticModel _model;
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
                var y = root.DescendantNodes();
                foreach (var classDeclarationSyntax in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    // ITypeSymbol symbol = ((ILocalSymbol)semanticModel.GetDeclaredSymbol(classDeclarationSyntax)).Type;
                    //var xx = semanticModel.GetTypeInfo(classDeclarationSyntax);
                    //if (InheritsFrom<DbContext>(symbol))
                    //{
                    //    var dataContextDecl = new DataContextDeclaration<EntityFramework>(classDeclarationSyntax.Identifier.Text, classDeclarationSyntax.GetCompilationInfo());
                    //    DataContextDeclarations.Add(dataContextDecl);
                    //}
                }
            }
        }

        private bool InheritsFrom<T>(ITypeSymbol symbol)
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
