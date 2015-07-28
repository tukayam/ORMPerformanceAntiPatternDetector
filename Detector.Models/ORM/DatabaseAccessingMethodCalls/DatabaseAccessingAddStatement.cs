using Detector.Models.Base;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using Detector.Models.Others;

namespace Detector.Models.ORM.DatabaseAccessingMethodCalls
{
    public class DatabaseAccessingAddStatement<T> : DatabaseAccessingMethodCallStatement<T> where T : ORMToolType
    {
        public DatabaseAccessingAddStatement(string queryTextInCSharp
          , ModelCollection<DatabaseEntityDeclaration<T>> entityDeclarations
          , CompilationInfo compilationInfo)
            : base(queryTextInCSharp, entityDeclarations, compilationInfo)
        { }
    }
}
