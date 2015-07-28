using Detector.Extractors.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseAccessingMethodCallExtractor : DatabaseAccessingMethodCallExtractor<LINQToSQL>
    {
        public LINQToSQLDatabaseAccessingMethodCallExtractor(Context<LINQToSQL> context)
            : base(context)
        { }
    }
}
