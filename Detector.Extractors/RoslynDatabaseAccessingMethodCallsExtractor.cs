using Detector.Extractors.Base;
using Detector.Models.ORM.Base;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Detector.Extractors
{
    public sealed class RoslynDatabaseAccessingMethodCallsExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallsExtractor
    {
        public IEnumerable<DatabaseAccessingMethodCallStatement> DatabaseAccessingMethodCalls { get; private set; }
        private SemanticModel model;
        public RoslynDatabaseAccessingMethodCallsExtractor(SemanticModel model)
            : base()
        {
            this.model = model;
            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            ObjectCreationExpressionSyntax sy = node.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();

            if (sy != null)
            {
                var ti = model.GetTypeInfo(sy);
            }
            var ty = model.GetTypeInfo(node);
            base.VisitVariableDeclaration(node);
        }

        /// <summary>
        /// Extracts DatabaseAccessingMethodCalls
        /// </summary>
        /// <param name="node"></param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var ty = model.GetTypeInfo(node);

            IEnumerable<QueryExpressionSyntax> queryExpressionsInsideInvocationExpression = node.DescendantNodes().OfType<QueryExpressionSyntax>();

            foreach (QueryExpressionSyntax queryExpression in queryExpressionsInsideInvocationExpression)
            {
                var x = model.GetTypeInfo(queryExpression).Type;
                var y = model.GetSymbolInfo(queryExpression);
            }

            base.VisitInvocationExpression(node);
        }
    }
}
