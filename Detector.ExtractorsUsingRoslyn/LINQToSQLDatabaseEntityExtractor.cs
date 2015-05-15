using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.ExtractorsUsingRoslyn
{
    public sealed class LINQToSQLDatabaseEntityExtractor
    {
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
