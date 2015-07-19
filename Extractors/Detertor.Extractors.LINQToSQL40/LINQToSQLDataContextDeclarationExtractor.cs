using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Detector.Extractors.LINQToSQL40
{
    public  class LINQToSQLDataContextDeclarationExtractor : CSharpSyntaxWalker
    {
        List<DataContextDeclaration<LINQToSQL>> DataContextDeclarations;

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.AttributeLists.ToString().Contains("DatabaseAttribute"))
            {
                DataContextDeclarations.Add(new DataContextDeclaration<LINQToSQL>(node.Identifier.ToString(), node.GetCompilationInfo()));
            }

            base.VisitClassDeclaration(node);
        }
    }
}
