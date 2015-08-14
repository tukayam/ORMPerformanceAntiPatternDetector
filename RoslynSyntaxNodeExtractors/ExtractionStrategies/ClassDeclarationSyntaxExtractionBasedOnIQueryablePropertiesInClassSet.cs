using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;
using Detector.Extractors.Base.ExtensionMethods;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Detector.Extractors.Base.ExtractionStrategies.ReturnTypes;
using Detector.Extractors.Base.ExtractionStrategies.Parameters;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DataContexts;

namespace Detector.Extractors.Base.ExtractionStrategies
{
    internal class ClassDeclarationSyntaxExtractionBasedOnIQueryablePropertiesInClassSet<T> : ExtractionStrategy<ClassExtractionReturnType> where T : ORMToolType
    {
        private SolutionParameter _solutionParameter;
        private DataContextDeclarationsParameter<T> _dataContextDeclarationsParameter;

        internal ClassDeclarationSyntaxExtractionBasedOnIQueryablePropertiesInClassSet(params Parameter[] parameters)
            : base(parameters)
        { }

        internal override void SetParameters(params Parameter[] parameters)
        {
            _solutionParameter = parameters.Where(p => p is SolutionParameter) as SolutionParameter;
            _dataContextDeclarationsParameter = parameters.Where(p => p is DataContextDeclarationsParameter<T>) as DataContextDeclarationsParameter<T>;
        }

        internal override async Task<HashSet<ClassExtractionReturnType>> Execute()
        {
            var result = new HashSet<ClassExtractionReturnType>();
            HashSet<DataContextDeclaration<T>> dataContextDeclarations = _dataContextDeclarationsParameter.Value;

            foreach (var dataContextDeclaration in dataContextDeclarations)
            {
                foreach (var propertyDeclarationSyntax in dataContextDeclaration.CompilationInfo.SyntaxNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                {
                    SemanticModel model = dataContextDeclaration.CompilationInfo.SemanticModel;
                    if (model.IsOfType<IQueryable>(propertyDeclarationSyntax))
                    {
                        TypeSyntax propertyType = propertyDeclarationSyntax.Type;
                        //Get T from DbSet<T> or IQueryable<T>
                        string entityClassName = (propertyType as GenericNameSyntax).TypeArgumentList.Arguments[0].ToFullString();

                        Dictionary<ClassDeclarationSyntax, SemanticModel> classes = await _solutionParameter.Value.GetClassesOfType(entityClassName);

                        if (classes.Keys.Count > 0)
                        {
                            foreach (var item in classes.Keys)
                            {
                                result.Add(new ClassExtractionReturnType(classes[item], item));
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
