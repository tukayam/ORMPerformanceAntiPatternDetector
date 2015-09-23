using Detector.Extractors.Base;
using Detector.Extractors.Base.ExtensionMethods;
using Detector.Extractors.Base.Helpers;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Threading.Tasks;

namespace Detector.Extractors.LINQToSQL40
{
    public class DatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<LINQToSQL>
    {
        public DatabaseEntityDeclarationExtractor(Context<LINQToSQL> context)
            : base(context)
        { }

        protected override async Task ExtractDatabaseEntityDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            Dictionary<ClassDeclarationSyntax, SemanticModel> classes = await solution.GetClassesSignedWithAttributeType<TableAttribute>();

            foreach (var item in classes.Keys)
            {
                var dbEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>(item.Identifier.ToString(), item.GetCompilationInfo(classes[item]));

                DatabaseEntityDeclarations.Add(dbEntityDeclaration);
            }
        }
    }
}
