using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DataContextInitializationStatement<T> : ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }
        public DataContextDeclaration<T> DataContextDeclarationUsed { get; private set; }

        public DataContextInitializationStatement(DataContextDeclaration<T> dataContextDeclarationUsed, CompilationInfo compilationUnit)
        {
            this.DataContextDeclarationUsed = dataContextDeclarationUsed;
            this.CompilationInfo = compilationUnit;
        }
    }
}
