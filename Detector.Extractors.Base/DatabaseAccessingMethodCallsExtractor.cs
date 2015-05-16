using Detector.Models.ORM.Base;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseAccessingMethodCallsExtractor
    {
        IEnumerable<DatabaseAccessingMethodCallStatement> DatabaseAccessingMethodCalls { get; }
    }
}
