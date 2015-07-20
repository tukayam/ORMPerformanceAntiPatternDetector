using Detector.Models.ORM;
using Detector.Models.Others;
using Detector.Extractors.Base;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public abstract class DataContextDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        private ModelCollection<DataContextDeclaration<T>> _dataContextDeclarations;

        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations
        {
            get
            {
                return _dataContextDeclarations;
            }
        }

        public DataContextDeclarationExtractor(Context<T> context)
            :base(context)
        {
            _dataContextDeclarations = new ModelCollection<DataContextDeclaration<T>>();
        }

        public async Task FindDataContextDeclarationsAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                await FindDataContextDeclarationsAsync(project);
            }
        }

        public async Task FindDataContextDeclarationsAsync(Project project)
        {
            await ExtractDataContextDeclarationsAsync(project);

            ConcreteContext<T>.Instance.DataContextDeclarations = DataContextDeclarations;
        }

        /// <summary>
        /// Concrete class must override this method.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public abstract Task ExtractDataContextDeclarationsAsync(Project project);
       
    }
}
