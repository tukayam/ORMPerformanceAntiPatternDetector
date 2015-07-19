using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseQueryExtractor<T> : Extractor where T : ORMToolType
    {
        ModelCollection<DatabaseQuery<T>> DatabaseQueries { get; }
    }
}
