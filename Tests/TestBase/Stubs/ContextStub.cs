using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace TestBase.Stubs
{
    public class ContextStub<T> : Context<T> where T : ORMToolType
    {
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }

        public ContextStub()
        {
            DataContextDeclarations = new ModelCollection<DataContextDeclaration<T>>();
            DatabaseEntityDeclarations = new ModelCollection<DatabaseEntityDeclaration<T>>();
        }
    }
}
