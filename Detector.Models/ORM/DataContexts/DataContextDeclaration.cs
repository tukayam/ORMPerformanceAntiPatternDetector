using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DataContexts
{
    public class DataContextDeclaration<T>:ModelBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo { get; private set; }
        public string Name { get; private set; }

        public DataContextDeclaration(string name, CompilationInfo compilationInfo)
        {
            this.Name = name;
            this.CompilationInfo = compilationInfo;
        }
    }
}
