using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseQueryVariable<T> : ModelBase where T : ORMToolType
    {
        //public string VariableName { get; private set; }
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQueryVariable(CompilationInfo compilationInfo)
        {
            // this.VariableName = variableName;
            this.CompilationInfo = compilationInfo;
        }
    }
}
