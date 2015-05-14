using System.Collections.Generic;

namespace Detector.Models.ORM.Base
{
    public abstract class Query
    {
        /// <summary>
        /// Entities that the query returns
        /// </summary>
        public IEnumerable<DatabaseEntityDeclaration> FetchedEntities;
        
        /// <summary>
        /// File in which the Query is found
        /// </summary>
        //public File File;
    }
}
