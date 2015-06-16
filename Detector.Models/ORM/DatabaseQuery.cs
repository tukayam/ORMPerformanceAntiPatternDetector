using System;
using System.Collections.Generic;
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseQuery<T> : ModelBase where T : ORMToolType
    {
        /// <summary>
        /// Entity declarations that the query uses
        /// </summary>
        public List<DatabaseEntityDeclaration<T>> EntityDeclarationsUsedInQuery { get; private set; }
        public DatabaseQueryVariable DatabaseQueryVariable { get; private set; }
        public string QueryAsString { get; private set; }

        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQuery(string queryAsString, List<DatabaseEntityDeclaration<T>> entityDeclarations, DatabaseQueryVariable databaseQueryVariable)
        {
            this.QueryAsString = queryAsString;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = databaseQueryVariable;
        }
    }
}
