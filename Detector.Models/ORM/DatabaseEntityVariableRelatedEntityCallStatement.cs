using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityVariableRelatedEntityCallStatement<T> : DatabaseEntityObjectCallStatement<T> where T:ORMToolType
    {
        public DatabaseEntityVariableRelatedEntityCallStatement(DatabaseEntityVariableDeclaration<T> databaseEntityObject, CompilationInfo compilationUnit)
            : base(databaseEntityObject, compilationUnit)
        { }
    }
}
