using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseAccessingMethodCallExtractor<T> where T : ORMToolType
    {
        List<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }
    }
}
