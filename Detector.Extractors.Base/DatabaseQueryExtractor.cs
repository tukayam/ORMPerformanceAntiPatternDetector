using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseQueryExtractor<T> where T : ORMToolType
    {
        IEnumerable<DatabaseQuery<T>> DatabaseQueries { get; }
    }
}
