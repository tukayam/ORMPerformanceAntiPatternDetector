using Detector.Models.ORM;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public interface Context<T> where T : ORMToolType
    {
        ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        ModelCollection<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; set; }
        ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
    }
}
