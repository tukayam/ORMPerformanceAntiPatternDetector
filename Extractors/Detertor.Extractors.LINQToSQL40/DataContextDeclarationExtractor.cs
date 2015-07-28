using Detector.Extractors.Base;
using Detector.Extractors.Base.Helpers;
using Detector.Extractors.Base.ExtensionMethods;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Threading.Tasks;
using System;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DataContexts;

namespace Detector.Extractors.LINQToSQL40
{
    public class DataContextDeclarationExtractor : DataContextDeclarationExtractor<LINQToSQL>
    {
        public DataContextDeclarationExtractor(Context<LINQToSQL> context)
            : base(context)
        { }

        protected override async Task ExtractDataContextDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            Dictionary<ClassDeclarationSyntax, SemanticModel> classes = await solution.GetClassesSignedWithAttributeType<DatabaseAttribute>();
            foreach (var classDeclarationSyntax in classes.Keys)
            {
                var dataContextDecl = new DataContextDeclaration<LINQToSQL>(classDeclarationSyntax.Identifier.ToString()
                                                                            , classDeclarationSyntax.GetCompilationInfo(classes[classDeclarationSyntax]));
                DataContextDeclarations.Add(dataContextDecl);
            }
        }
    }
}
