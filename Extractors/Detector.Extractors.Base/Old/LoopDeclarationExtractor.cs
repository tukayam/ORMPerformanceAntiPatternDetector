using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public abstract class LoopDeclarationExtractor<T> : Extractor<T> where T:ORMToolType
    {
        public abstract HashSet<LoopDeclarationBase> LoopDeclarations { get; }

        public LoopDeclarationExtractor(Context<T> context)
            : base(context)
        { }
    }
}
