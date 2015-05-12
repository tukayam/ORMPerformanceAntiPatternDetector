using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Models
{
    public abstract class Query
    {
        /// <summary>
        /// Entities that the query returns
        /// </summary>
        public IEnumerable<Entity> FetchedEntities;
        
        /// <summary>
        /// File in which the Query is found
        /// </summary>
        //public File File;
    }
}
