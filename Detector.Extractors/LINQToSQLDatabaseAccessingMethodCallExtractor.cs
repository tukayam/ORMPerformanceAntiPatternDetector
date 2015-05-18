using Detector.Extractors.Base;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors
{
    public class LINQToSQLDatabaseAccessingMethodCallExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }
     
        private readonly List<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public LINQToSQLDatabaseAccessingMethodCallExtractor(SemanticModel model
            , DatabaseEntityDeclarationExtractor<LINQToSQL> databaseEntityDeclarationsExtractor)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarations = databaseEntityDeclarationsExtractor.DatabaseEntityDeclarations.ToList();

            this._databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax>();
            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>>();
            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            var dbAccessingMethodCalls = (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                                          where _databaseQueries.ContainsKey(q)
                                          select new DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>(_databaseQueries[q], new CompilationInfo("", "", 0)));

            this.DatabaseAccessingMethodCalls.AddRange(dbAccessingMethodCalls.ToList());
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            var variableDeclarationSyntax = from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                            from v in _databaseQueryVariables.Keys
                                            where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                                            select v;

            if (variableDeclarationSyntax.FirstOrDefault() != null && variableDeclarationSyntax.Count() == 1)
            {
                this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatementOnQueryVariable<LINQToSQL>(_databaseQueries[_databaseQueryVariables[variableDeclarationSyntax.First()]], new CompilationInfo("", "", 0)));
            }
        }
    }
}
