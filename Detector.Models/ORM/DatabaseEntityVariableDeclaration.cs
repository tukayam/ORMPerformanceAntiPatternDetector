using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityVariableDeclaration<T> : VariableDeclaration, ModelBase where T : ORMToolType
    {
        public DatabaseEntityVariableDeclaration(string variableName, CompilationInfo compilationInfo)
            : base(variableName, compilationInfo)
        { }
    }
}
