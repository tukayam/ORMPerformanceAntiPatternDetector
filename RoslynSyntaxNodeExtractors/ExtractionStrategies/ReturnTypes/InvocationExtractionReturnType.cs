using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Base.ExtractionStrategies.ReturnTypes
{
    internal class InvocationExtractionReturnType : ExtractionReturnTypeBase
    {
        public InvocationExpressionSyntax InvocationExpressionSyntax { get; private set; }

        public InvocationExtractionReturnType(SemanticModel semanticModel,
                                         InvocationExpressionSyntax invocationExpressionSyntax)
            : base(semanticModel)
        {
            InvocationExpressionSyntax = invocationExpressionSyntax;
        }
    }
}
