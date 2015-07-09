using Detector.Models.ORM;

namespace Detector.Extractors.Base
{
    public interface ExtractorFactory<T> where T:ORMToolType
    {
        DatabaseAccessingMethodCallsExtractor<T> GetDatabaseAccessingMethodCallsExtractor();
        DatabaseEntities.DatabaseEntityDeclarationExtractor<T> GetDatabaseEntityDeclarationExtractor();
    }
}
