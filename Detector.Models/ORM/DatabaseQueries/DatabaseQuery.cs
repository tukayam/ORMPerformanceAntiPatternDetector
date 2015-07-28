using Detector.Models.Base;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using Detector.Models.Others;

namespace Detector.Models.ORM.DatabaseQueries
{
    public class DatabaseQuery<T> : ModelBase where T : ORMToolType
    {
        /// <summary>
        /// Entity declarations that the query uses
        /// </summary>
        public ModelCollection<DatabaseEntityDeclaration<T>> EntityDeclarationsUsedInQuery { get; private set; }
        public DatabaseQueryVariable<T> DatabaseQueryVariable { get; private set; }
        public string QueryTextInCSharp { get; private set; }

        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQuery(string queryTextInCSharp
            , ModelCollection<DatabaseEntityDeclaration<T>> entityDeclarations
            , DatabaseQueryVariable<T> databaseQueryVariable
            , CompilationInfo compilationInfo)
        {
            this.QueryTextInCSharp = queryTextInCSharp;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = databaseQueryVariable;
            this.CompilationInfo = compilationInfo;
        }
    }
}
