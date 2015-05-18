using Detector.Models.Base;

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
