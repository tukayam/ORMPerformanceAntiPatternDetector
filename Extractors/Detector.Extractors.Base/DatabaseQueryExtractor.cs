using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseQueryExtractor<T> : Extractor<T> where T : ORMToolType
    {
        ModelCollection<DatabaseQuery<T>> DatabaseQueries { get; }

        public DatabaseQueryExtractor(Context<T> context)
            : base(context)
        { }
    }
}
