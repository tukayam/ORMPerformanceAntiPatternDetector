using Detector.Models.ORM;
using Detector.Models.ORM.LINQToSQL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Detector.Extractors.DatabaseEntities
{
    public sealed class RoslynLINQToSQLDatabaseEntityExtractor : CSharpSyntaxWalker, DatabaseEntityExtractor<LINQToSQLEntity>
    {
        private List<LINQToSQLEntity> entities;
        public IEnumerable<LINQToSQLEntity> Entities
        {
            get { return entities; }
        }

        public RoslynLINQToSQLDatabaseEntityExtractor()
            : base()
        {
            entities = new List<LINQToSQLEntity>();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.AttributeLists.ToString().Contains("TableAttribute"))
            {
                entities.Add(new LINQToSQLEntity(node.Identifier.ToString()) { });
            }
            ORMContext.Entities = entities;
            base.VisitClassDeclaration(node);
        }
    }
}
