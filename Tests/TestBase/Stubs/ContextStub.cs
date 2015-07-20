using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace TestBase.Stubs
{
    public class ContextStub<T> : Context<T> where T : ORMToolType
    {
        private ModelCollection<DataContextDeclaration<T>> _dataContextDeclarations;
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations
        {
            get
            {
                return _dataContextDeclarations;
            }

            set
            {
                _dataContextDeclarations = value;
            }
        }
    }
}
