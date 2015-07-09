using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DataContextDeclarationExtractor<T> : Extractor where T : ORMToolType
    {
        ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; }
    }
}
