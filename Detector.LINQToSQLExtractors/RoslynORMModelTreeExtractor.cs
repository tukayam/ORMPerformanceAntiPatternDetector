using Detector.Models;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;
using Detector.Extractors.Helpers;
using Detector.Models.Others;
using Detector.Extractors.Base;

namespace Detector.Extractors
{
    /// <summary>
    /// Generates an ORMModelTree for a given MethodDeclarationSyntax
    /// </summary>
    public class RoslynORMModelTreeExtractor : ORMModelTreeExtractor
    {
        private ORMModelTree _ORMModelTree;
        private IEnumerable<DatabaseQuery<LINQToSQL>> _databaseQueries;

        public RoslynORMModelTreeExtractor(IEnumerable<DatabaseQuery<LINQToSQL>> databaseQueries)
        {
            this._databaseQueries = databaseQueries;
        }

        public ORMModelTree Extract(MethodDeclarationSyntax methodDeclarationSyntaxNode)
        {
            var methodDeclaration = new MethodDeclaration(methodDeclarationSyntaxNode.Identifier.Text, methodDeclarationSyntaxNode.GetCompilationInfo());
            this._ORMModelTree = new ORMModelTree(new ORMModelNode(methodDeclaration));

            var extractor = new LINQToSQLDatabaseAccessingMethodCallExtractor(null, null, _databaseQueries.ToList());
            extractor.Visit(methodDeclarationSyntaxNode);

            foreach (var dbAccessingMethodCall in extractor.DatabaseAccessingMethodCalls)
            {
                var ORMModelNode = new ORMModelNode(dbAccessingMethodCall);
                _ORMModelTree.RootNode.ChildNodes.Add(ORMModelNode);
            }

            return _ORMModelTree;
        }

    }
}
