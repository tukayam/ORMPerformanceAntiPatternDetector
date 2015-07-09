using Detector.Models;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public interface DatabaseAccessingLoopDeclarationExtractor<T> :Extractor where T : ORMToolType
    {
        ModelCollection<DatabaseAccessingLoopDeclaration<T>> DatabaseAccessingLoopDeclarations { get; }
    }
}
