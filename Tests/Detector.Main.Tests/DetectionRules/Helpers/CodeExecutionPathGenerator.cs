using Detector.Main.Tests.Stubs;
using Detector.Models.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using System.Collections.Generic;

namespace Detector.Main.Tests.DetectionRules.Helpers
{
    class CodeExecutionPathGenerator
    {
        private CodeExecutionPath _codeExecutionPath;

        DatabaseEntityDeclaration<FakeORMToolType> customerEntityDeclaration = new DatabaseEntityDeclaration<FakeORMToolType>("Customer", null);
        DatabaseEntityDeclaration<FakeORMToolType> orderEntityDeclaration = new DatabaseEntityDeclaration<FakeORMToolType>("Order", null);
        DatabaseAccessingMethodCallStatement<FakeORMToolType> dbAccessingMethodCall;
        DatabaseEntityVariableDeclaration<FakeORMToolType> databaseEntityVariableDeclaration = new DatabaseEntityVariableDeclaration<FakeORMToolType>("customer1", null);

        internal CodeExecutionPathGenerator()
        {
            _codeExecutionPath = new CodeExecutionPath();
        }

        internal CodeExecutionPathGenerator WithLazyLoadingDatabaseAccessingMethodCall()
        {
            GetDatabaseAccessingMethodCall();

            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<FakeORMToolType>>() { customerEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            _codeExecutionPath.AddModel(dbAccessingMethodCall);
            return this;
        }

        internal CodeExecutionPathGenerator WithEagerLoadingDatabaseAccessingMethodCall()
        {
            GetDatabaseAccessingMethodCall();
            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<FakeORMToolType>>() { customerEntityDeclaration, orderEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            _codeExecutionPath.AddModel(dbAccessingMethodCall);

            return this;
        }

        internal CodeExecutionPathGenerator WithDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
        {
            dbAccessingMethodCall.SetAssignedVariable(databaseEntityVariableDeclaration);
            return this;
        }

        private DatabaseAccessingMethodCallStatement<FakeORMToolType> GetDatabaseAccessingMethodCall()
        {
            var entityDeclarationsUsedInQuery = new HashSet<DatabaseEntityDeclaration<FakeORMToolType>>() { customerEntityDeclaration };

            string queryText = "(from c in dc.Customers where c.Id=1 select c)";

            dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<FakeORMToolType>(queryText, entityDeclarationsUsedInQuery, null);

            return dbAccessingMethodCall;
        }

        internal CodeExecutionPathGenerator WithCallToRelatedEntityOnDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
        {
            var databaseEntityRelatedObjectCall = new DatabaseEntityVariableRelatedEntityCallStatement<FakeORMToolType>(databaseEntityVariableDeclaration, null);

            _codeExecutionPath.AddModel(databaseEntityRelatedObjectCall);

            return this;
        }

        internal CodeExecutionPath Build()
        {
            return _codeExecutionPath;
        }
    }
}
