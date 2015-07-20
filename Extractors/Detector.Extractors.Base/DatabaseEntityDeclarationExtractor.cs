using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.DatabaseEntities
{
    public abstract class DatabaseEntityDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public abstract ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; }

        public abstract Task FindDatabaseEntityDeclarationsAsync(Solution solution);

        public DatabaseEntityDeclarationExtractor(Context<T> context)
            : base(context)
        { }
    }
}
