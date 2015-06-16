using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Models;
using System;
using Detector.Models.Base;
using Detector.Models.Others;

namespace Detector.Extractors
{
    public class LINQToSQLLoopDeclarationExtractor : CSharpSyntaxWalker, DatabaseAccessingLoopDeclarationExtractor<LINQToSQL>, LoopDeclarationExtractor
    {
        public List<DatabaseAccessingLoopDeclaration<LINQToSQL>> DatabaseAccessingLoopDeclarations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<LoopDeclarationBase> LoopDeclarations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;


        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            DatabaseAccessingForeachLoopDeclaration<LINQToSQL> dbAccessingForEach =
                (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                 from v in _databaseQueryVariables.Keys
                 where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                 select new DatabaseAccessingForeachLoopDeclaration<LINQToSQL>()).FirstOrDefault();

            if (dbAccessingForEach != null)
            {
                DatabaseAccessingLoopDeclarations.Add(dbAccessingForEach);
            }
            else
            {
                LoopDeclarations.Add(new ForEachLoopDeclaration());
            }

            base.VisitForEachStatement(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            DatabaseAccessingForLoopDeclaration<LINQToSQL> dbAccessingFor =
                (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                 from v in _databaseQueryVariables.Keys
                 where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                 select new DatabaseAccessingForLoopDeclaration<LINQToSQL>()).FirstOrDefault();

            if (dbAccessingFor != null)
            {
                DatabaseAccessingLoopDeclarations.Add(dbAccessingFor);
            }
            else
            {
                LoopDeclarations.Add(new ForLoopDeclaration());
            }
            base.VisitForStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            LoopDeclarations.Add(new WhileLoopDeclaration());

            base.VisitWhileStatement(node);
        }

    }
}
