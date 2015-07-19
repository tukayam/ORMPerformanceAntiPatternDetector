using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Detector.Extractors.EF602
{
    public  class DataContextDeclarationExtractor : CSharpSyntaxWalker
    {
        SemanticModel _model;
        List<DataContextDeclaration<LINQToSQL>> DataContextDeclarations;

        public DataContextDeclarationExtractor(SemanticModel model)
        {
            _model = model;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
           // if(((ILocalSymbol)_model.GetDeclaredSymbol(node)).Type == typeof())

            if (node.AttributeLists.ToString().Contains("DatabaseAttribute"))
            {
                DataContextDeclarations.Add(new DataContextDeclaration<LINQToSQL>(node.Identifier.ToString(), node.GetCompilationInfo()));
            }

            base.VisitClassDeclaration(node);
        }
    }
}
