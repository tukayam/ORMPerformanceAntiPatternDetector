using Detector.Models.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public interface ISerializer<T> where T : ORMToolType
    {
        Task Serialize(HashSet<DataContextDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(HashSet<DatabaseEntityDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(HashSet<DatabaseQueryVariableDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(HashSet<DatabaseAccessingMethodCallStatement<T>> collection, string solutionUnderTest);
        Task Serialize(HashSet<CodeExecutionPath> collection, string solutionUnderTest);
    }
}
