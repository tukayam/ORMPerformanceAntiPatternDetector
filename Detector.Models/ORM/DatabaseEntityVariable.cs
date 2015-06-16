using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityVariable<T> : VariableDeclaration, ModelBase where T : ORMToolType
    {
        public DatabaseEntityVariable(string variableName, CompilationInfo compilationInfo)
            : base(variableName, compilationInfo)
        { }
    }
}
