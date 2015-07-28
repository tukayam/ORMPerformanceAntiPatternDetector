using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Detector.Extractors.Base.Helpers;
using Detector.Extractors.Base.ExtensionMethods;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DatabaseEntities;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<LINQToSQL>
    {
        public LINQToSQLDatabaseEntityDeclarationExtractor(Context<LINQToSQL> context)
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
