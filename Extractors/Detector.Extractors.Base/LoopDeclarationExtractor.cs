using Detector.Models.Base;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public abstract class LoopDeclarationExtractor<T> : Extractor<T> where T:ORMToolType
    {
        public abstract ModelCollection<LoopDeclarationBase> LoopDeclarations { get; }

        public LoopDeclarationExtractor(Context<T> context)
            : base(context)
        { }
    }
}
