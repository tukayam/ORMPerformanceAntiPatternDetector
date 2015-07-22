using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public abstract class DataContextDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; protected set; }

        public DataContextDeclarationExtractor(Context<T> context)
            : base(context)
        {
            DataContextDeclarations = new ModelCollection<DataContextDeclaration<T>>();
        }

        public async Task FindDataContextDeclarationsAsync(Solution solution)
        {
            await ExtractDataContextDeclarationsAsync(solution);

            Context.DataContextDeclarations = DataContextDeclarations;
        }

        protected abstract Task ExtractDataContextDeclarationsAsync(Solution solution);
    }
}
