using Detector.Models;
using Microsoft.CodeAnalysis;

namespace Detector.Main
{
    public interface SyntaxTreeWalker
    {
        ORMModelTree ORMSyntaxTree { get; }

        void Visit(SyntaxNode syntaxNode);
    }
}
