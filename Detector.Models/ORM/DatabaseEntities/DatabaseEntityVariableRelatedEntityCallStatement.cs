using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseEntities
{
    public class DatabaseEntityVariableRelatedEntityCallStatement<T> : DatabaseEntityObjectCallStatement<T> where T:ORMToolType
    {
        public DatabaseEntityVariableRelatedEntityCallStatement(DatabaseEntityVariableDeclaration<T> databaseEntityObject, CompilationInfo compilationUnit)
            : base(databaseEntityObject, compilationUnit)
        { }
    }
}
