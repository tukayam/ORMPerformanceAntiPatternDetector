using Detector.Models.Base;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Models.ORM.DatabaseQueries
{
    public class DatabaseQuery<T> : ModelBase where T : ORMToolType
    {
        /// <summary>
        /// Entity declarations that the query uses
        /// </summary>
        public HashSet<DatabaseEntityDeclaration<T>> EntityDeclarationsUsedInQuery { get; private set; }
        public DatabaseQueryVariableDeclaration<T> DatabaseQueryVariable { get; private set; }
        public string QueryTextInCSharp { get; private set; }

        public DatabaseQuery(string queryTextInCSharp
            , HashSet<DatabaseEntityDeclaration<T>> entityDeclarations
            , DatabaseQueryVariableDeclaration<T> databaseQueryVariable
            , CompilationInfo compilationInfo)
            :base(compilationInfo)
        {
            this.QueryTextInCSharp = queryTextInCSharp;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = databaseQueryVariable;
        }
    }
}
