using System.Collections.Generic;

namespace Detector.Models.ORM
{
    public class DatabaseQuery<T> where T : ORMToolType
    {
        /// <summary>
        /// Entity declarations that the query uses
        /// </summary>
        public IEnumerable<DatabaseEntityDeclaration<T>> EntityDeclarations { get; private set; }

        public string QueryAsString { get; private set; }

        public DatabaseQuery(string queryAsString, IEnumerable<DatabaseEntityDeclaration<T>> entityDeclarations)
        {
            this.QueryAsString = queryAsString;
            this.EntityDeclarations = entityDeclarations;
        }
    }
}
