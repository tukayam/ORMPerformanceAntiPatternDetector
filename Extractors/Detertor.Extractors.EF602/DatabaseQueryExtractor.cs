using Detector.Extractors.Base;
using Detector.Models.ORM;

namespace Detector.Extractors.EF602
{
    public class DatabaseQueryExtractor : DatabaseQueryExtractor<EntityFramework>
    {
        public DatabaseQueryExtractor(Context<EntityFramework> context)
            : base(context)
        { }
    }
}
