using Detector.Models.Compilation;

namespace Detector.Models.ORM
{
    public abstract class DatabaseEntityObjectCallStatement<T> : ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseEntityObject<T> DatabaseEntityObject { get; private set; }

        public DatabaseEntityObjectCallStatement(CompilationInfo compilationUnit, DatabaseEntityObject<T> databaseEntityObject)
        {
            this.CompilationInfo = compilationUnit;
            this.DatabaseEntityObject = databaseEntityObject;
        }
    }
}
