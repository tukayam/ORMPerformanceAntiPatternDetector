using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
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

        public static bool IsOfType<T>(this SemanticModel model, PropertyDeclarationSyntax syntax)
        {
            IPropertySymbol symbol = model.GetDeclaredSymbol(syntax);

            return InheritsFrom<T>(symbol);
        }

        public static bool IsOfType(this SemanticModel model, ClassDeclarationSyntax syntax, string typeName)
        {
            INamedTypeSymbol symbol = model.GetDeclaredSymbol(syntax);

            return InheritsFrom(symbol, typeName);
        }

        public static bool IsOfType(this SemanticModel model, SyntaxNode node, IEnumerable<string> types)
        {
            ITypeSymbol typeOfNode = model.GetTypeInfo(node).Type;
            if (typeOfNode != null)
            {
                return InheritsFrom(typeOfNode, types);
            }
            return false;
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

        private static bool InheritsFrom(ITypeSymbol symbolType, IEnumerable<string> typesToCheck)
        {
            bool result = false;
            foreach (var typeToCheck in typesToCheck)
            {
                result = InheritsFrom(symbolType, typeToCheck);
                if (result == true)
                {
                    break;
                }
            }
            return result;
        }

        private static bool InheritsFrom(ITypeSymbol symbolType, string type)
        {
            string typeToCheck = type.Split('.').Last();

            while (true)
            {
                if (symbolType.ToString().Split('.').Last().Equals(typeToCheck))
                {
                    return true;
                }
                else if (symbolType.AllInterfaces.Any(x => x.ToString().Split('.').Last().Equals(typeToCheck)))
                {
                    return true;
                }
                else if (symbolType.BaseType != null
                    && !symbolType.BaseType.ToString().Split('.').Last().Equals("Object"))
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
