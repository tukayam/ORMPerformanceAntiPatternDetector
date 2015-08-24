using Detector.Models.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface Context<T> where T : ORMToolType
    {
        HashSet<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        HashSet<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        HashSet<DatabaseQueryVariableDeclaration<T>> DatabaseQueryVariables { get; set; }
        HashSet<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
        HashSet<CodeExecutionPath> CodeExecutionPaths { get; set; }
    }
}
