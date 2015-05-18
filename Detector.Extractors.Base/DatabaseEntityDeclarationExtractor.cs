using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.DatabaseEntities
{
    public interface DatabaseEntityDeclarationExtractor<T> where T : ORMToolType
    {
        IEnumerable<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; }
    }
}
