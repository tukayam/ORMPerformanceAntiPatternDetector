using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Detector.Extractors.DatabaseEntities
{
    public sealed class LINQToSQLDatabaseEntityExtractor : CSharpSyntaxWalker, DatabaseEntityDeclarationsExtractor<LINQToSQL>
    {
        private List<DatabaseEntityDeclaration<LINQToSQL>> _entities;

        public IEnumerable<DatabaseEntityDeclaration<LINQToSQL>> EntityDeclarations
        {
            get
            {
                return _entities;
            }
        }

        public LINQToSQLDatabaseEntityExtractor()
            : base()
        {
            _entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.AttributeLists.ToString().Contains("TableAttribute"))
            {
                _entities.Add(new DatabaseEntityDeclaration<LINQToSQL>(node.Identifier.ToString()) { });
            }

            base.VisitClassDeclaration(node);
        }
    }
}
