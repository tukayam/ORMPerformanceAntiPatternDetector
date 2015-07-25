using Detector.Extractors.Base;
using Detector.Models.ORM;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseQueryExtractor : DatabaseQueryExtractor<LINQToSQL>
    {
        public LINQToSQLDatabaseQueryExtractor(Context<LINQToSQL> context)
            : base(context)
        { }
    }
}
