using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DataContextDeclarationExtractor<T> where T : ORMToolType
    {
        IEnumerable<DataContextDeclaration<T>> DataContextDeclarations { get; }
    }
}
