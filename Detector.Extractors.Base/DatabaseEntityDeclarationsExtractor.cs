using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.DatabaseEntities
{
    public interface DatabaseEntityDeclarationsExtractor<T> where T : ORMToolType
    {
        IEnumerable<DatabaseEntityDeclaration<T>> EntityDeclarations { get; }
    }
}
