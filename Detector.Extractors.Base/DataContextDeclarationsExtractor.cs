using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DataContextDeclarationsExtractor<T> where T : ORMToolType
    {
        IEnumerable<DataContextDeclaration<T>> DataContextDeclarations { get; }
    }
}
