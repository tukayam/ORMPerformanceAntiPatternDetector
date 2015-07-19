using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;
using System.Data.Linq;

namespace Detector.Extractors.EF602
{
    public class DatabaseContextInitializationStatementExtractor : CSharpSyntaxWalker
    {
        private readonly List<DataContextDeclaration<LINQToSQL>> _dataContextDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<DataContextInitializationStatement<LINQToSQL>, List<DatabaseEntityVariableDeclaration<LINQToSQL>>> _dataContextInitializationStatementsAndLoadedDatabaseEntityDeclarations;
        private List<VariableDeclarationSyntax> _dataContextVariables;
        public List<DataContextInitializationStatement<LINQToSQL>> _dataContextInitializationStatements { get; private set; }
        private List<VariableDeclarationSyntax> _dataLoadOptionsVariables;

        public DatabaseContextInitializationStatementExtractor(SemanticModel model
                        , List<DataContextDeclaration<LINQToSQL>> dataContextDeclarations)
            : base()
        {
            this._model = model;
            this._dataContextDeclarations = dataContextDeclarations;
            
            this._dataContextInitializationStatements = new List<DataContextInitializationStatement<LINQToSQL>>();
            this._dataContextInitializationStatementsAndLoadedDatabaseEntityDeclarations = new Dictionary<DataContextInitializationStatement<LINQToSQL>, List<DatabaseEntityVariableDeclaration<LINQToSQL>>>();
            this._dataLoadOptionsVariables = new List<VariableDeclarationSyntax>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            ITypeSymbol typeOfNode = _model.GetTypeInfo(node).Type;
            if (typeOfNode.Equals(typeof(DataLoadOptions)))
            {
                this._dataLoadOptionsVariables.Add(node);
            }
            else
            {
                DataContextDeclaration<LINQToSQL> dataContextDeclaration = _dataContextDeclarations.Where(x => x.Name == typeOfNode.ToString()).FirstOrDefault();

                if (dataContextDeclaration != null)
                {
                    this._dataContextVariables.Add(node);
                    var dataContextInitializationStatement = new DataContextInitializationStatement<LINQToSQL>(dataContextDeclaration, node.GetCompilationInfo());
                    this._dataContextInitializationStatements.Add(dataContextInitializationStatement);
                }
            }
            base.VisitVariableDeclaration(node);
        }
    }
}
