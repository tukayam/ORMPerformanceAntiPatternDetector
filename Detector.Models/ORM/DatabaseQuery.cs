using System.Collections.Generic;

namespace Detector.Models.ORM
{
    public class DatabaseQuery<T> where T : ORMToolType
    {
        public DatabaseQueryVariable DatabaseQueryVariable { get; private set; }
        /// <summary>
        /// Entity declarations that the query uses
        /// </summary>
        public List<DatabaseEntityDeclaration<T>> EntityDeclarationsUsedInQuery { get; private set; }

        public string QueryAsString { get; private set; }

        public DatabaseQuery(string queryAsString, List<DatabaseEntityDeclaration<T>> entityDeclarations)
        {
            this.QueryAsString = queryAsString;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = null;
        }

        public DatabaseQuery(string queryAsString, List<DatabaseEntityDeclaration<T>> entityDeclarations, DatabaseQueryVariable databaseQueryVariable)
        {
            this.QueryAsString = queryAsString;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = databaseQueryVariable;
        }
    }
}
