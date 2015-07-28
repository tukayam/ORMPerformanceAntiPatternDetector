using Detector.Models;
using Detector.Models.ORM.DatabaseAccessingLoops;
using Detector.Models.ORM.ORMTools;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseAccessingLoopDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        ModelCollection<DatabaseAccessingLoopDeclaration<T>> DatabaseAccessingLoopDeclarations { get; }

        public DatabaseAccessingLoopDeclarationExtractor(Context<T> context)
            :base(context)
        {   }
    }
}
