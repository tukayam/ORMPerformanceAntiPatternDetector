using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseAccessingMethodCallsExtractor<T> : Extractor where T : ORMToolType
    {
        ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }
    }
}
