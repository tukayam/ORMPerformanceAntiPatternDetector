using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Detector.Extractors.Base.ExtensionMethods
{
    public static class RoslynSemanticModelExtensions
    {
        public static bool IsOfType<T>(this SemanticModel model, ClassDeclarationSyntax syntax)
        {
            INamedTypeSymbol symbol = model.GetDeclaredSymbol(syntax);

            return InheritsFrom<T>(symbol);
        }

        public static bool IsOfType(this SemanticModel model, ClassDeclarationSyntax syntax, string typeName)
        {
            INamedTypeSymbol symbol = model.GetDeclaredSymbol(syntax);

            return InheritsFrom(symbol, typeName);
        }

        public static bool IsOfType<T>(this SemanticModel model, PropertyDeclarationSyntax syntax)
        {
            IPropertySymbol symbol = model.GetDeclaredSymbol(syntax);

            return InheritsFrom<T>(symbol);
        }

        private static bool InheritsFrom<T>(INamedTypeSymbol symbol)
        {
            return InheritsFrom(symbol, typeof(T).ToString());
        }

        private static bool InheritsFrom<T>(IPropertySymbol symbol)
        {
            ITypeSymbol symbolType = symbol.Type;
            return InheritsFrom(symbolType, typeof(T).ToString());
        }

        private static bool InheritsFrom(ITypeSymbol symbolType, string typeToCheck)
        {
            while (true)
            {
                if (symbolType.ToString().EndsWith(typeToCheck))
                {
                    return true;
                }
                else if (symbolType.AllInterfaces.Any(x => x.ToString().EndsWith(typeToCheck)))
                {
                    return true;
                }
                else if (symbolType.BaseType != null
                    && !symbolType.BaseType.ToString().EndsWith("Object"))
                {
                    symbolType = symbolType.BaseType;
                    continue;
                }
                break;
            }
            return false;
        }
    }
}
