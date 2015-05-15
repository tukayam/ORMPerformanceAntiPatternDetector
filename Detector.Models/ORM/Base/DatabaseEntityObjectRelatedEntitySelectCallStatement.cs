using Detector.Models.Compilation;

namespace Detector.Models.ORM.Base
{
    public class DatabaseEntityObjectRelatedEntitySelectCallStatement : DatabaseEntityObjectCallStatement
    {
        public DatabaseEntityObjectRelatedEntitySelectCallStatement(CompilationUnit compilationUnit, DatabaseEntityObject databaseEntityObject)
            : base(compilationUnit, databaseEntityObject)
        { }

        public DatabaseEntityObject RelatedEntityObject { get; set; }
    }
}
