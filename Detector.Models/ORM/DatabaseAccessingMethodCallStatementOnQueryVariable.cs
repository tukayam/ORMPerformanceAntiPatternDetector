using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatementOnQueryVariable<T> : DatabaseAccessingMethodCallStatement<T>, ModelBase where T : ORMToolType
    {
        public DatabaseQueryVariable DatabaseQueryVariable { get; private set; }

        public DatabaseAccessingMethodCallStatementOnQueryVariable(DatabaseQuery<T> databaseQuery, CompilationInfo compilationInfo, DatabaseQueryVariable databaseQueryVariable)
            : base(databaseQuery, compilationInfo)
        {
            this.DatabaseQueryVariable = databaseQueryVariable;
        }
    }
}
