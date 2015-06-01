using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Helpers
{
    public static class DatabaseQueryExtensions
    {
        public static bool IsSameQueryAs<T>(this DatabaseQuery<T> databaseQuery, QueryExpressionSyntax queryExpressionSyntax)
            where T : ORMToolType
        {
            return databaseQuery.QueryAsString == queryExpressionSyntax.GetText().ToString();
        }
    }
}
