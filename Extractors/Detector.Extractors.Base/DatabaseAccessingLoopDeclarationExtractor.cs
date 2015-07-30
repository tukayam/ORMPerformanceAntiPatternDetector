using Detector.Models.ORM.DatabaseAccessingLoops;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseAccessingLoopDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        HashSet<DatabaseAccessingLoopDeclaration<T>> DatabaseAccessingLoopDeclarations { get; }

        public DatabaseAccessingLoopDeclarationExtractor(Context<T> context)
            :base(context)
        {   }
    }
}
