using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.DatabaseEntities
{
    public abstract class DatabaseEntityDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; }
       
        public async Task FindDatabaseEntityDeclarationsAsync(Solution solution)
        {
            await ExtractDatabaseEntityDeclarationsAsync(solution);
        }

        protected abstract Task ExtractDatabaseEntityDeclarationsAsync(Solution solution);

        public DatabaseEntityDeclarationExtractor(Context<T> context)
            : base(context)
        {
            DatabaseEntityDeclarations = new ModelCollection<DatabaseEntityDeclaration<T>>();
        }
    }
}
