using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;
using System.Data.Linq;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDataLoadOptionsExtractor : CSharpSyntaxWalker
    {
        private readonly SemanticModel _model;

        public List<VariableDeclarationSyntax> _dataLoadOptionsVariables { get; private set; }

        public LINQToSQLDataLoadOptionsExtractor(SemanticModel model)
            : base()
        {
            this._model = model;
            this._dataLoadOptionsVariables = new List<VariableDeclarationSyntax>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            ITypeSymbol typeOfNode = _model.GetTypeInfo(node).Type;
            if (typeOfNode.Equals(typeof(DataLoadOptions)))
            {
                this._dataLoadOptionsVariables.Add(node);
            }

            base.VisitVariableDeclaration(node);
        }
    }
}
