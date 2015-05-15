using Detector.Models.Compilation;

namespace Detector.Models.ORM.Base
{
    public class DataContextInitializationStatement : ModelBase
    {
        public CompilationUnit CompilationUnit
        {
            get;
            private set;
        }

        public DataContextInitializationStatement(CompilationUnit compilationUnit)
        {
            this.CompilationUnit = compilationUnit;
        }
    }
}
