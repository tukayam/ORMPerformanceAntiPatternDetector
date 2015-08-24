using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseQueries
{
    public class DatabaseQueryVariableDeclaration<T> : Model where T : ORMToolType
    {
        public string VariableName { get; private set; }
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseQueryVariableDeclaration(string variableName, CompilationInfo compilationInfo)
        {
            this.VariableName = variableName;
            this.CompilationInfo = compilationInfo;
        }
    }
}
