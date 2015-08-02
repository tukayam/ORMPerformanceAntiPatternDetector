using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseEntities
{
    public class DatabaseEntityVariableDeclaration<T> : VariableDeclaration where T : ORMToolType
    {
        public DatabaseEntityVariableDeclaration(string variableName, CompilationInfo compilationInfo)
            : base(variableName, compilationInfo)
        { }
    }
}
