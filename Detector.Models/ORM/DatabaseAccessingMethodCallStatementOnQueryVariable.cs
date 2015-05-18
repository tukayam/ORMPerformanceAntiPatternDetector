using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatementOnQueryVariable<T> : DatabaseAccessingMethodCallStatement<T>, ModelBase where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatementOnQueryVariable(DatabaseQuery<T> databaseQuery, CompilationInfo compilationInfo)
            : base(databaseQuery, compilationInfo)
        { }
    }
}
