using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public abstract class DataContextDeclaration<T> where T : ORMToolType
    {
        public CompilationInfo CompilationInfo
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public DataContextDeclaration(string name)
        {
            this.Name = name;
        }
    }
}
