using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.Base;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Detector.Extractors.Base.ExtensionMethods;
using Detector.Models.Base;
using Detector.Extractors.Base.Helpers;
using System.Collections.Generic;
using System;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DatabaseEntities;

namespace Detector.Extractors.EF602
{
    public class DatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<EntityFramework>
    {
        public DatabaseEntityDeclarationExtractor(Context<EntityFramework> context)
            : base(context)
        { }

        protected override async Task ExtractDatabaseEntityDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            string extractionNote = "Extracting Database Entity Declarations by finding all IQueryable<T> properties in Data Context Declarations";
            progress.Report(new ExtractionProgress(extractionNote));
            int totalAmountOfDataContextClasses = GetTotalAmountOfDataContextClasses();

            int counter = 0;
            foreach (var dataContextClassDeclarationSyntax in Context.DataContextDeclarations)
            {
                counter++;
                progress.Report(GetExtractionProgress(totalAmountOfDataContextClasses, counter));

                CompilationInfo compInfo = dataContextClassDeclarationSyntax.CompilationInfo;
                foreach (var propertyDeclarationSyntax in compInfo.SyntaxNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                {
                    SemanticModel model = compInfo.SemanticModel;
                    if (model.IsOfType<IQueryable>(propertyDeclarationSyntax))
                    {
                        TypeSyntax propertyType = propertyDeclarationSyntax.Type;
                        //Get T from DbSet<T> or IQueryable<T>
                        string entityClassName = (propertyType as GenericNameSyntax).TypeArgumentList.Arguments[0].ToFullString();

                        Dictionary<ClassDeclarationSyntax, SemanticModel> entityClass = await solution.GetClassesOfType(entityClassName, progress);

                        if (entityClass.Keys.Count > 0)
                        {
                            ClassDeclarationSyntax entityClassDeclarationSyntax = entityClass.Keys.First();
                            SemanticModel modelForEntityClass = entityClass[entityClassDeclarationSyntax];
                            DatabaseEntityDeclarations.Add(new DatabaseEntityDeclaration<EntityFramework>(entityClassName,
                                entityClassDeclarationSyntax.GetCompilationInfo(modelForEntityClass)));
                        }
                    }
                }
            }
        }

        private int _totalAmountOfDataContextClasses;
        private int GetTotalAmountOfDataContextClasses()
        {
            if (_totalAmountOfDataContextClasses == 0)
            {
                _totalAmountOfDataContextClasses = Context.DataContextDeclarations.Count;
            }
            return _totalAmountOfDataContextClasses;
        }

        private ExtractionProgress GetExtractionProgress(int totalAmountOfDataContextClasses, int counter)
        {
            int percentage = counter * 100 / totalAmountOfDataContextClasses;

            return new ExtractionProgress(percentage);
        }
    }
}
