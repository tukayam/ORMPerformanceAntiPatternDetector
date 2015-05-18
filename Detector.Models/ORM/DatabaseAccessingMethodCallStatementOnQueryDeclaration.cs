
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatementOnQueryDeclaration<T> : DatabaseAccessingMethodCallStatement<T>, ModelBase where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatementOnQueryDeclaration(DatabaseQuery<T> databaseQuery, CompilationInfo compilationInfo)
            : base(databaseQuery, compilationInfo)
        { }
    }
}
