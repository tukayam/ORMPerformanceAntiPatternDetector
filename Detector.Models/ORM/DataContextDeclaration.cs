using Detector.Models.Base;

namespace Detector.Models.ORM
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
