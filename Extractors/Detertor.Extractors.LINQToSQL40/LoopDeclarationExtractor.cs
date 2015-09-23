using Detector.Extractors.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System;
using Detector.Models.Base;
using Detector.Models.Others;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DatabaseAccessingLoops;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLLoopDeclarationExtractor : DatabaseAccessingLoopDeclarationExtractor<LINQToSQL>
    {
        public HashSet<DatabaseAccessingLoopDeclaration<LINQToSQL>> DatabaseAccessingLoopDeclarations
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

        public LINQToSQLLoopDeclarationExtractor(Context<LINQToSQL> context)
            :base(context)
        {

        }

        public void VisitForEachStatement(ForEachStatementSyntax node)
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
            
        }

        public void VisitForStatement(ForStatementSyntax node)
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
        }

        public  void VisitWhileStatement(WhileStatementSyntax node)
        {
            LoopDeclarations.Add(new WhileLoopDeclaration());            
        }

    }
}
