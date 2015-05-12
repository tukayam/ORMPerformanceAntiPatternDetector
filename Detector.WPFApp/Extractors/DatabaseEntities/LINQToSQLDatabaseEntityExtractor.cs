using Detector.WPFApp.Models.ORM.LINQToSQL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Extractors.DatabaseEntities
{
    public sealed class LINQToSQLDatabaseEntityExtractor : CSharpSyntaxWalker, DatabaseEntityExtractor<LINQToSQLEntity>
    {
        private List<LINQToSQLEntity> entities;
        public IEnumerable<LINQToSQLEntity> Entities
        {
            get { return entities; }
        }

        public LINQToSQLDatabaseEntityExtractor()
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
            base.VisitClassDeclaration(node);
        }
    }
}
