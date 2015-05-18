using Detector.Models;
using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface DatabaseAccessingLoopDeclarationExtractor<T> where T : ORMToolType
    {
        List<DatabaseAccessingLoopDeclaration<T>> DatabaseAccessingLoopDeclarations { get; }
    }
}
