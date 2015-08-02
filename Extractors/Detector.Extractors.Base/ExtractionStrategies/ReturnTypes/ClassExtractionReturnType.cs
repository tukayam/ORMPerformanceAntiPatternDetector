using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Base.ExtractionStrategies.ReturnTypes
{
    public sealed class ClassExtractionReturnType : ExtractionReturnTypeBase
    {
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; private set; }

        public ClassExtractionReturnType(SemanticModel semanticModel,
                                        ClassDeclarationSyntax classDeclarationSyntax)
            : base(semanticModel)
        {
            ClassDeclarationSyntax = classDeclarationSyntax;
        }

    }
}
