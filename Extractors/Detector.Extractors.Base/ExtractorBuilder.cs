using Detector.Extractors.Base.ExtractionStrategies;
using Detector.Extractors.Base.ExtractionStrategies.Parameters;
using Detector.Extractors.Base.ExtractionStrategies.ReturnTypes;
using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public class ExtractorBuilder<T> where T : ORMToolType
    {
        private SolutionParameter _solutionParameter;

        private ExtractionStrategy<ClassExtractionReturnType> _dataContextDeclarationExtractionStrategy;
        private ExtractionStrategy<ClassExtractionReturnType> _databaseEntityDeclarationExtractionStrategy;
        private ExtractionStrategy _databaseAccessingMethodCallStatementExtractionStrategy;

        private HashSet<DataContextDeclaration<T>> _dataContextDeclarations;
        private HashSet<DatabaseEntityDeclaration<T>> _databaseEntityDeclarations;

        public ExtractorBuilder(Solution solution)
        {
            _solutionParameter = new SolutionParameter("solution", solution);
        }

        public ExtractorBuilder<T> WithDbEntitiesDerivedFromBaseClass(string baseClassName)
        {
            var baseClassNameParameter = new StringParameter("baseClassName", baseClassName);

            _databaseEntityDeclarationExtractionStrategy = new ClassDeclarationSyntaxExtractionBasedOnClassDerivation<T>(_solutionParameter, baseClassNameParameter);
            return this;
        }

        public ExtractorBuilder<T> WithDbEntitiesAsGenericTypeOnIQueryablesInDataContextClasses()
        {
            var dataContextDeclarationsParameter = new DataContextDeclarationsParameter<T>("dataContextClassDeclarations", _dataContextDeclarations);
            _dataContextDeclarationExtractionStrategy = new ClassDeclarationSyntaxExtractionBasedOnIQueryablePropertiesInClassSet<T>(_solutionParameter, dataContextDeclarationsParameter);
            return this;
        }

        public async Task Extract()
        {
            var resultDataContextExtractions = await _dataContextDeclarationExtractionStrategy.Execute();
            _dataContextDeclarations = new HashSet<DataContextDeclaration<T>>();
            foreach (var item in resultDataContextExtractions)
            {
                var dataContextDeclaration = new DataContextDeclaration<T>(item.ClassDeclarationSyntax.Identifier.ToString(), item.ClassDeclarationSyntax.GetCompilationInfo(item.SemanticModel));
                _dataContextDeclarations.Add(dataContextDeclaration);
            }

            var resultDatabaseEntities = await _databaseEntityDeclarationExtractionStrategy.Execute();
            _databaseEntityDeclarations = new HashSet<DatabaseEntityDeclaration<T>>();
            foreach (var item in resultDatabaseEntities)
            {
                var databaseEntityDeclaration = new DatabaseEntityDeclaration<T>(item.ClassDeclarationSyntax.Identifier.ToString(), item.ClassDeclarationSyntax.GetCompilationInfo(item.SemanticModel));
                _databaseEntityDeclarations.Add(databaseEntityDeclaration);
            }

            //await _databaseAccessingMethodCallStatementExtractionStrategy.Execute(parameters);
        }
    }
}
