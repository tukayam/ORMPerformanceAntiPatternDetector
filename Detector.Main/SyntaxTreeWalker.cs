using Detector.Models;
using Microsoft.CodeAnalysis;

namespace Detector.Main
{
    public interface SyntaxTreeWalker
    {
        ORMSyntaxTree ORMSyntaxTree { get; }

        void Visit(SyntaxNode syntaxNode);
    }
}
