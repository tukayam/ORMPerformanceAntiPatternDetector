namespace Detector.Models.Base
{
    public class VariableDeclaration : ModelBase
    {
        public string VariableName { get; private set; }
        public CompilationInfo CompilationInfo { get; private set; }

        public VariableDeclaration(string variableName, CompilationInfo compilationInfo)
        {
            VariableName = variableName;
            CompilationInfo = compilationInfo;
        }
    }
}
