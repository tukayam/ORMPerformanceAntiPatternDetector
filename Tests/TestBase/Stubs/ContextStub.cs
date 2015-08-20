using Detector.Extractors.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;
using Detector.Models.Base;

namespace TestBase.Stubs
{
    public class ContextStub<T> : Context<T> where T : ORMToolType
    {
        public HashSet<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public HashSet<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        public HashSet<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
        public HashSet<DatabaseQuery<T>> DatabaseQueries { get; set; }
        public HashSet<DatabaseQueryVariableDeclaration<T>> DatabaseQueryVariables { get; set; }
        public HashSet<CodeExecutionPath> CodeExecutionPaths { get; set; }

        public ContextStub()
        {
            DataContextDeclarations = new HashSet<DataContextDeclaration<T>>();
            DatabaseEntityDeclarations = new HashSet<DatabaseEntityDeclaration<T>>();
            DatabaseAccessingMethodCallStatements = new HashSet<DatabaseAccessingMethodCallStatement<T>>();
            DatabaseQueries = new HashSet<DatabaseQuery<T>>();
            DatabaseQueryVariables = new HashSet<DatabaseQueryVariableDeclaration<T>>();
        }
    }
}
