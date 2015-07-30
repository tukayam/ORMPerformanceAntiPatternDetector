using Detector.Models.Base;
using Detector.Models.ORM;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using Detector.Models.Others;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public interface ISerializer<T> where T : ORMToolType
    {
        Task Serialize(ModelCollection<DataContextDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseEntityDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseQueryVariable<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseAccessingMethodCallStatement<T>> collection, string solutionUnderTest);
        Task Serialize(HashSet<CodeExecutionPath> collection, string solutionUnderTest);
    }
}
