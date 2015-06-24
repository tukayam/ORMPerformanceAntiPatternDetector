
using Detector.Models.Base;

namespace Detector.Models.ORM
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
