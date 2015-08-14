using Detector.Extractors.Base.ExtractionStrategies.ReturnTypes;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.Base.ExtractionStrategies
{
    internal class InvocationExpressionSyntaxExtractionBasedOnExistenceOfDescendantNodeInType<T> : ExtractionStrategy<InvocationExtractionReturnType> where T : ORMToolType
    {
        private SolutionParameter _solutionParameter;
        private StringParameter _derivedFromTypeNameParameter;

        internal InvocationExpressionSyntaxExtractionBasedOnExistenceOfDescendantNodeInType(params Parameter[] parameters)
            : base(parameters)
        { }

        internal override void SetParameters(params Parameter[] parameters)
        {
            _solutionParameter = parameters.Where(p => p is SolutionParameter) as SolutionParameter;
            _derivedFromTypeNameParameter = parameters.Where(p => p is StringParameter) as StringParameter;
        }

        internal override async Task<HashSet<InvocationExtractionReturnType>> Execute()
        {
            return null;
        }
    }
}
