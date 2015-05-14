using Detector.Models.ORM.Base;
using System.Collections.Generic;

namespace Detector.Extractors.DatabaseEntities
{
    public interface DatabaseEntityExtractor<T> where T : DatabaseEntityDeclaration
    {
        IEnumerable<T> Entities { get; }
    }
}
