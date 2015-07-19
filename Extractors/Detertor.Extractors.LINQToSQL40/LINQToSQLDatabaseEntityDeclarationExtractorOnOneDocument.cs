using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Detector.Extractors.LINQToSQL40
{
    public sealed class LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument : CSharpSyntaxWalker
    {
        private List<DatabaseEntityDeclaration<LINQToSQL>> _entities;

        public List<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations
        {
            get
            {
                return _entities;
            }
        }

        public LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument()
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
