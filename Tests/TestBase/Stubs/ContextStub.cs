using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace TestBase.Stubs
{
    public class ContextStub<T> : Context<T> where T : ORMToolType
    {
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        public ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
        public ModelCollection<DatabaseQuery<T>> DatabaseQueries { get; set; }
        public ModelCollection<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; set; }

        public ContextStub()
        {
            DataContextDeclarations = new ModelCollection<DataContextDeclaration<T>>();
            DatabaseEntityDeclarations = new ModelCollection<DatabaseEntityDeclaration<T>>();
            DatabaseAccessingMethodCallStatements = new ModelCollection<DatabaseAccessingMethodCallStatement<T>>();
            DatabaseQueries = new ModelCollection<DatabaseQuery<T>>();
            DatabaseQueryVariables = new ModelCollection<DatabaseQueryVariable<T>>();
        }
    }
}
