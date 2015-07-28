
using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseEntities
{
    public abstract class DatabaseEntityObjectCallStatement<T> : ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseEntityVariableDeclaration<T> CalledDatabaseEntityVariable { get; private set; }

        public DatabaseEntityObjectCallStatement(DatabaseEntityVariableDeclaration<T> calledDatabaseEntityVariable, CompilationInfo compilationUnit)
        {
            this.CompilationInfo = compilationUnit;
            this.CalledDatabaseEntityVariable = calledDatabaseEntityVariable;
        }
    }
}
