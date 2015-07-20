using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseAccessingMethodCallsExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }

        public DatabaseAccessingMethodCallsExtractor(Context<T> context)
            : base(context)
        { }
    }
}
