using Detector.Models.Compilation;

namespace Detector.Models.ORM
{
    public class DataContextInitializationStatement : ModelBase
    {
        public CompilationInfo CompilationInfo
        {
            get;
            private set;
        }

        public DataContextInitializationStatement(CompilationInfo compilationUnit)
        {
            this.CompilationInfo = compilationUnit;
        }
    }
}
