using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityObjectRelatedEntitySelectCallStatement<T> : DatabaseEntityObjectCallStatement<T> where T:ORMToolType
    {
        public DatabaseEntityObjectRelatedEntitySelectCallStatement(CompilationInfo compilationUnit, DatabaseEntityObject<T> databaseEntityObject)
            : base(compilationUnit, databaseEntityObject)
        { }

        public DatabaseEntityObject<T> RelatedEntityObject { get; set; }
    }
}
