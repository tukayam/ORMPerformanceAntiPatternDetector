using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;
using Detector.Extractors.Base.ExtensionMethods;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DataContexts;
using System.Linq;
using Detector.Extractors.Base.ExtractionStrategies.ReturnTypes;

namespace Detector.Extractors.Base.ExtractionStrategies
{
    internal class ClassDeclarationSyntaxExtractionBasedOnAttributeSignature<T> : ExtractionStrategy<ClassExtractionReturnType> where T : ORMToolType
    {
        private SolutionParameter _solutionParameter;
        private StringParameter _derivedFromTypeNameParameter;

        internal ClassDeclarationSyntaxExtractionBasedOnAttributeSignature(params Parameter[] parameters)
            : base(parameters)
        { }

        internal override void SetParameters(params Parameter[] parameters)
        {
            _solutionParameter = parameters.Where(p => p is SolutionParameter) as SolutionParameter;
            _derivedFromTypeNameParameter = parameters.Where(p => p is StringParameter) as StringParameter;
        }

        internal override async Task<HashSet<ClassExtractionReturnType>> Execute()
        {
            var dataContextDeclarations = new HashSet<DataContextDeclaration<T>>();
            Dictionary<ClassDeclarationSyntax, SemanticModel> classes = await _solutionParameter.Value.GetClassesSignedWithAttributeType(_derivedFromTypeNameParameter.Value);

            var result = new HashSet<ClassExtractionReturnType>();
            foreach (var item in classes.Keys)
            {
                result.Add(new ClassExtractionReturnType(classes[item], item));
            }

            return result;
        }
    }
}
