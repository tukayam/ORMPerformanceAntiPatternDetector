using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Entity;
using System.Threading.Tasks;
using Detector.Extractors.Base;
using Detector.Extractors.Base.ExtensionMethods;
using System.Collections.Generic;
using System;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DataContexts;

namespace Detector.Extractors.EF602
{
    public class DataContextDeclarationExtractor : DataContextDeclarationExtractor<Detector.Models.ORM.ORMTools.EntityFramework>
    {
        public DataContextDeclarationExtractor(Context<EntityFramework> context)
            : base(context)
        {

        }

        protected override async Task ExtractDataContextDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            string extractionNote = "Extracting Data Context Declarations by finding classes of type DbContext";
            progress.Report(new ExtractionProgress(extractionNote));

            Dictionary<ClassDeclarationSyntax,SemanticModel> classes = await solution.GetClassesOfType<DbContext>();

            foreach (var item in classes.Keys)
            {
                DataContextDeclarations.Add(new DataContextDeclaration<EntityFramework>(item.Identifier.ToString(), item.GetCompilationInfo(classes[item])));
            }
        }
    }
}
