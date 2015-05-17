using Detector.Models.Compilation;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatement<T> : Models.ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQuery<T> DatabaseQuery { get; private set; }

        public DatabaseAccessingMethodCallStatement(DatabaseQuery<T> databaseQuery)
        {
            this.DatabaseQuery = databaseQuery;
        }
    }
}
