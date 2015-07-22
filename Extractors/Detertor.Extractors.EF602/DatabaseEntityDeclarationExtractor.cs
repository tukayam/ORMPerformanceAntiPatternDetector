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

namespace Detector.Extractors.EF602
{
    public class DatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<EntityFramework>
    {
        public DatabaseEntityDeclarationExtractor(Context<EntityFramework> context)
            : base(context)
        { }

        protected override async Task ExtractDatabaseEntityDeclarationsAsync(Solution solution)
        {
            foreach (var dataContextClassDeclarationSyntax in Context.DataContextDeclarations)
            {
                CompilationInfo compInfo = dataContextClassDeclarationSyntax.CompilationInfo;
                foreach (var propertyDeclarationSyntax in compInfo.SyntaxNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                {
                    SemanticModel model = compInfo.SemanticModel;
                    if (model.IsOfType<IQueryable>(propertyDeclarationSyntax))
                    {
                        TypeSyntax propertyType = propertyDeclarationSyntax.Type;
                        //Get T from DbSet<T> or IQueryable<T>
                        string entityClassName = (propertyType as GenericNameSyntax).TypeArgumentList.Arguments[0].ToFullString();

                        Dictionary<ClassDeclarationSyntax, SemanticModel> entityClass = await solution.GetClassesOfType(entityClassName);

                        if (entityClass.Count != 1)
                        {
                            throw new Exception(String.Format("EntityClass for type {0} was not found correctly. Instead found class signatures are: {1}", entityClassName, entityClass.Keys.ToString()));
                        }

                        ClassDeclarationSyntax entityClassDeclarationSyntax = entityClass.Keys.First();
                        SemanticModel modelForEntityClass = entityClass[entityClassDeclarationSyntax];
                        DatabaseEntityDeclarations.Add(new DatabaseEntityDeclaration<EntityFramework>(entityClassName,
                            entityClassDeclarationSyntax.GetCompilationInfo(modelForEntityClass)));
                    }
                }
            }
        }
    }
}
