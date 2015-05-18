
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public abstract class DatabaseAccessingMethodCallStatement<T> : Models.ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQuery<T> DatabaseQuery { get; private set; }

        public DatabaseAccessingMethodCallStatement(DatabaseQuery<T> databaseQuery, CompilationInfo compilationInfo)
        {
            this.DatabaseQuery = databaseQuery;
            this.CompilationInfo = compilationInfo;
        }
    }
}
