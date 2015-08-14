using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Base.ExtractionStrategies.ReturnTypes
{
    internal class VariableExtractionReturnType:ExtractionReturnTypeBase
    {
        public VariableDeclarationSyntax VariableDeclarationSyntax { get; private set; }

        public VariableExtractionReturnType(SemanticModel semanticModel,
                                         VariableDeclarationSyntax variableDeclarationSyntax)
            : base(semanticModel)
        {
            VariableDeclarationSyntax = variableDeclarationSyntax;
        }
    }
}
